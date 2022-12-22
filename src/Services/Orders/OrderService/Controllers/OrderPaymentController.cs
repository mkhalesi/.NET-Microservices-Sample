using System;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.Services.OrderService;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentController : ControllerBase
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
