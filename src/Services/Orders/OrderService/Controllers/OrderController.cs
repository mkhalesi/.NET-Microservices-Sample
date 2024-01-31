using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using OrderService.Model.Services.OrderService;

namespace OrderService.Controllers
{
    public class OrderController : OrderSiteBaseController
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // identify from Claims
            string userId = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
            var orders = orderService.GetOrdersForUser(userId);
            return Ok(orders);
        }

        [HttpGet("{OrderId}")]
        public IActionResult Get(string OrderId)
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
