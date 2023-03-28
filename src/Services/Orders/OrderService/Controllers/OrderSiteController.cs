using Microsoft.AspNetCore.Mvc;
using System;
using OrderService.Model.Services.OrderService;

namespace OrderService.Controllers
{
    public class OrderSiteController : OrderSiteBaseController
    {
        private readonly IOrderService orderService;
        public OrderSiteController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // identify from Claims
            string UserId = "1";
            var orders = orderService.GetOrdersForUser(UserId);
            return Ok(orders);
        }

        [HttpGet("{OrderId}")]
        public IActionResult Get(Guid OrderId)
        {
            var order = orderService.GetOrderById(OrderId);
            return Ok(order);
        }

        // [HttpPost]
        // public IActionResult Post([FromBody] AddOrderDto order)
        // {
        //     orderService.AddOrder(order);
        //     return Ok();
        // }

    }
}
