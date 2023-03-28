using System;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.Services.OrderService;

namespace OrderService.Controllers
{
    public class OrderSitePaymentController : OrderSiteBaseController
    {
        private readonly IOrderService orderService;
        public OrderSitePaymentController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult Post(Guid orderId)
        {
            return Ok(orderService.RequestPayment(orderId));
        }
    }
}
