using System;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.Services.OrderService;

namespace OrderService.Controllers
{
    public class OrderPaymentController : OrderSiteBaseController
    {
        private readonly IOrderService orderService;
        public OrderPaymentController(IOrderService orderService)
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
