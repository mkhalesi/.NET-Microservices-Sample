using System;
using System.Linq;
using AutoMapper;
using BasketService.Extensions;
using BasketService.Infrastructure.Contexts;
using BasketService.MessageBus.Config;
using BasketService.MessageBus.SendMessages;
using BasketService.Model.DTOs;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.DTOs.Discount;
using BasketService.Model.DTOs.MessageDTO;
using BasketService.Model.DTOs.Product;
using BasketService.Model.Entities;
using BasketService.Model.Services.CacheService;
using BasketService.Model.Services.DiscountService;
using BasketService.Model.Services.ProductService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BasketService.Model.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly BasketDataBaseContext context;
        private readonly IMapper mapper;
        private IMessageBus messageBus;
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;
        private readonly string queueName_BasketCheckout;
        public BasketService(BasketDataBaseContext context, IMapper mapper,
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IMessageBus messageBus,
            ICacheService cacheService,
            IProductService productService)
        {
            this.context = context;
            this.mapper = mapper;
            this.messageBus = messageBus;
            _cacheService = cacheService;
            _productService = productService;
            queueName_BasketCheckout = rabbitMqOptions.Value.QueueName_BasketCheckout;
        }

        public void AddItemToBasket(AddItemToBasketDto item)
        {
            // Todo: UserId property new Added
            var basketItem = _cacheService.GetList<BasketItem>(ConstantExtension.GetBasketItemListKey(item.UserId));
            if (basketItem == null)
                throw new Exception("Basket not found....!");

            var productDto = mapper.Map<ProductDTO>(item);
            basketItem.Add(mapper.Map<BasketItem>(item));

            _productService.CreateProduct(productDto);

            _cacheService.SetList<BasketItem>(ConstantExtension.GetBasketItemListKey(item.UserId), basketItem);
        }

        public BasketDto GetBasket(string userId)
        {
            var basket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(userId));
            if (basket == null)
            {
                return null;
            }

            var basketModel = new BasketDto()
            {
                Id = basket.Id,
                UserId = basket.UserId,
            };
            foreach (var item in basket.Items)
            {
                var product = _productService.GetProduct(item.ProductId);
                basketModel.Items.Add(new BasketItemDto()
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,

                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    ImageUrl = product.ImageUrl
                });
            }

            return basketModel;
        }

        public BasketDto GetOrCreateBasketForUser(string userId)
        {
            var basket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(userId));
            if (basket == null)
            {
                return CreateBasketForUser(userId);
            }

            var basketModel = new BasketDto()
            {
                Id = basket.Id,
                UserId = basket.UserId,
            };
            foreach (var item in basket.Items)
            {
                var product = _productService.GetProduct(item.ProductId);
                basketModel.Items.Add(new BasketItemDto()
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,

                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    ImageUrl = product.ImageUrl
                });
            }

            return basketModel;
        }

        public void RemoveItemFromBasket(string itemId, string userId)
        {
            var basketItems = _cacheService.GetList<BasketItem>(ConstantExtension.GetBasketItemListKey(userId));
            if (basketItems == null)
                throw new Exception("BasketItem Not Found...!");

            basketItems.Remove(basketItems.FirstOrDefault(p => p.Id == itemId));

            _cacheService.SetList<BasketItem>(ConstantExtension.GetBasketItemListKey(userId), basketItems);
        }

        public void SetQuantities(string itemId, int quantity, string userId)
        {
            var basketItems = _cacheService.GetList<BasketItem>(ConstantExtension.GetBasketItemListKey(userId));

            basketItems.FirstOrDefault(p => p.Id == itemId)?.SetQuantity(quantity);

            _cacheService.SetList<BasketItem>(ConstantExtension.GetBasketItemListKey(userId), basketItems);
        }

        public void TransferBasket(string anonymousId, string userId)
        {
            var anonymousBasket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(anonymousId));
            if (anonymousBasket == null) return;

            var userBasket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(userId));
            if (userBasket == null)
            {
                userBasket = new Basket(userId);

                foreach (var item in anonymousBasket.Items)
                {
                    userBasket.Items.Add(new BasketItem
                    {
                        BasketId = userBasket.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    });
                }

                _cacheService.Set(ConstantExtension.GetBasketKey(userId), userBasket);
            }

            _cacheService.Remove(ConstantExtension.GetBasketKey(anonymousId));
        }

        private BasketDto CreateBasketForUser(string userId)
        {
            Basket basket = new Basket(userId);

            _cacheService.Set<Basket>(ConstantExtension.GetBasketKey(userId), basket);

            return new BasketDto
            {
                UserId = basket.UserId,
                Id = basket.Id,
            };
        }

        public void ApplyDiscountToBasket(string basketId, string discountId, string userId)
        {
            var basket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(userId));

            if (basket == null)
                throw new Exception("Basket not found....!");

            basket.DiscountId = discountId;
            _cacheService.Set<Basket>(ConstantExtension.GetBasketKey(userId), basket);
        }

        public ResultDTO CheckoutBasket(CheckoutBasketDTO checkoutBasket, IDiscountService discountService)
        {
            var basket = _cacheService.Get<Basket>(ConstantExtension.GetBasketKey(checkoutBasket.UserId));
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
            if (!string.IsNullOrEmpty(basket.DiscountId))
                discount = discountService.GetDiscountById(Guid.Parse(basket.DiscountId));
            if (discount != null)
                message.TotalPrice -= discount.Amount;

            //sending message
            messageBus.SendMessage(message, queueName_BasketCheckout);

            //Delete basket
            _cacheService.Remove(ConstantExtension.GetBasketKey(basket.UserId));

            return new ResultDTO()
            {
                IsSuccess = true,
                Message = "Order saved successfully"
            };
        }

    }
}
