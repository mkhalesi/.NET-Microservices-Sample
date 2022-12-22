using System;
using System.Linq;
using AutoMapper;
using BasketService.Infrastructure.Contexts;
using BasketService.MessageBus;
using BasketService.MessageBus.Config;
using BasketService.MessageBus.SendMessages;
using BasketService.Model.DTOs;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.DTOs.Discount;
using BasketService.Model.DTOs.MessageDTO;
using BasketService.Model.DTOs.Product;
using BasketService.Model.Entities;
using BasketService.Model.Services.DiscountService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BasketService.Model.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly BasketDataBaseContext context;
        private readonly IMapper mapper;
        private IMessageBus messageBus;
        private readonly string queueName_BasketCheckout;
        public BasketService(BasketDataBaseContext context, IMapper mapper,
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IMessageBus messageBus)
        {
            this.context = context;
            this.mapper = mapper;
            this.messageBus = messageBus;
            queueName_BasketCheckout = rabbitMqOptions.Value.QueueName_BasketCheckout;
        }

        public void AddItemToBasket(AddItemToBasketDto item)
        {
            var basket = context.Baskets.FirstOrDefault(p => p.Id == item.basketId);

            if (basket == null)
                throw new Exception("Basket not found....!");

            var productDto = mapper.Map<ProductDTO>(item);
            var basketItem = mapper.Map<BasketItem>(item);

            createProduct(productDto);

            basket.Items.Add(basketItem);
            context.SaveChanges();
        }

        private ProductDTO getProduct(Guid productId)
        {
            var existsProduct = context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (existsProduct == null)
                return null;

            return mapper.Map<ProductDTO>(existsProduct);
        }

        private ProductDTO createProduct(ProductDTO product)
        {
            var existsProduct = getProduct(product.ProductId);
            if (existsProduct != null)
                return existsProduct;

            var newProduct = mapper.Map<Product>(product);
            context.Products.Add(newProduct);
            context.SaveChanges();

            return mapper.Map<ProductDTO>(newProduct);
        }

        public BasketDto GetBasket(string UserId)
        {
            var basket = context.Baskets
                .Include(p => p.Items)
                .ThenInclude(p => p.Product)
                .SingleOrDefault(p => p.UserId == UserId);

            if (basket == null)
            {
                return null;
            }
            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Id = item.Id,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.UnitPrice,
                    ImageUrl = item.Product.ImageUrl
                }).ToList(),
            };
        }

        public BasketDto GetOrCreateBasketForUser(string UserId)
        {

            var basket = context.Baskets
                .Include(p => p.Items)
                .ThenInclude(p => p.Product)
                .SingleOrDefault(p => p.UserId == UserId);
            if (basket == null)
            {
                return CreateBasketForUser(UserId);
            }

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Id = item.Id,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.UnitPrice,
                    ImageUrl = item.Product.ImageUrl,
                }).ToList(),
            };
        }

        public void RemoveItemFromBasket(Guid ItemId)
        {
            var item = context.BasketItems.SingleOrDefault(p => p.Id == ItemId);
            if (item == null)
                throw new Exception("BasketItem Not Found...!");
            context.BasketItems.Remove(item);
            context.SaveChanges();
        }

        public void SetQuantities(Guid itemId, int quantity)
        {
            var item = context.BasketItems.SingleOrDefault(p => p.Id == itemId);
            item.SetQuantity(quantity);
            context.SaveChanges();
        }

        public void TransferBasket(string anonymousId, string UserId)
        {
            var anonymousBasket = context.Baskets
                .Include(p => p.Items)
                .SingleOrDefault(p => p.UserId == anonymousId);

            if (anonymousBasket == null) return;

            var userBasket = context.Baskets.SingleOrDefault(p => p.UserId == UserId);
            if (userBasket == null)
            {
                userBasket = new Basket(UserId);
                context.Baskets.Add(userBasket);
            }
            foreach (var item in anonymousBasket.Items)
            {
                userBasket.Items.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    //UnitPrice = item.Product.UnitPrice,
                    //ProductName = item.Product.ProductName,
                    //ImageUrl = item.Product.ImageUrl,
                });
            }
            context.Baskets.Remove(anonymousBasket);
            context.SaveChanges();
        }

        private BasketDto CreateBasketForUser(string UserId)
        {
            Basket basket = new Basket(UserId);
            context.Baskets.Add(basket);
            context.SaveChanges();
            return new BasketDto
            {
                UserId = basket.UserId,
                Id = basket.Id,
            };
        }

        public ResultDTO CheckoutBasket(CheckoutBasketDTO checkoutBasket, IDiscountService discountService)
        {
            var basket = context.Baskets
                .Include(p => p.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(p => p.Id == checkoutBasket.BasketId);
            if (basket == null)
                return new ResultDTO()
                {
                    IsSuccess = false,
                    ErrorMessage = $"{nameof(basket)} Not Found!"
                };

            //sending message to RabbitMQ , send to Order
            BasketCheckoutMessage message = mapper.Map<BasketCheckoutMessage>(checkoutBasket);
            foreach (var item in basket.Items)
            {
                message.TotalPrice += item.Product.UnitPrice * item.Quantity;
                message.BasketItems.Add(new BasketItemMessage()
                {
                    BasketItemId = item.BasketId,
                    ProductId = item.ProductId,
                    Name = item.Product.ProductName,
                    Price = item.Product.UnitPrice,
                    Quantity = item.Quantity,
                });
            }

            // getting discount from Discount Service
            DiscountDTO discount = new DiscountDTO();
            if (basket.DiscountId.HasValue)
                discount = discountService.GetDiscountById(basket.DiscountId.Value);
            if (discount != null)
                message.TotalPrice = message.TotalPrice - discount.Amount;

            //sending message
            messageBus.SendMessage(message , queueName_BasketCheckout);

            //Delete basket
            context.Baskets.Remove(basket);
            context.SaveChanges();
            return new ResultDTO()
            {
                IsSuccess = true,
                Message = "Order saved successfully"
            };
        }

    }
}
