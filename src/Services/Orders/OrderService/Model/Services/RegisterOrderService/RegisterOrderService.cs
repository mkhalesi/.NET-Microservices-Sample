using System.Collections.Generic;
using System.Linq;
using OrderService.Infrastructure.Context;
using OrderService.Model.DTOs.Basket;
using OrderService.Model.Entities;
using OrderService.Model.Services.ProductService;

namespace OrderService.Model.Services.RegisterOrderService
{
    public class RegisterOrderService : IRegisterOrderService
    {
        private readonly IProductService productService;
        private readonly OrderDataBaseContext context;
        public RegisterOrderService(OrderDataBaseContext context, IProductService productService)
        {
            this.productService = productService;
            this.context = context;
        }

        public bool Execute(BasketDTO basket)
        {
            List<OrderLine> orderLines = new List<OrderLine>();
            Order order = new Order(
                basket.UserId,
                basket.FirstName,
                basket.LastName,
                basket.Address,
                basket.PhoneNumber,
                basket.TotalPrice,
                orderLines);
            context.Orders.Add(order);
            context.SaveChanges();

            if (basket.BasketItems != null && basket.BasketItems.Any())
            {
                foreach (var item in basket.BasketItems)
                {
                    var product = productService.GetProduct(new DTOs.Product.ProductDTO
                    {
                        ProductName = item.Name,
                        ProductPrice = item.Price,
                        ProductId = item.ProductId
                    });
                    orderLines.Add(new OrderLine()
                    {
                        Id = System.Guid.NewGuid(),
                        Quantity = item.Quantity,
                        ProductId = product.ProductId,
                        OrderId = order.Id
                    });
                };
                context.SaveChanges();
            }

            return true;
        }
    }
}