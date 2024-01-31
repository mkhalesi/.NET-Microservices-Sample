using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OrdersManagement")]
    public class OrdersManagementController : ControllerBase
    {
        [HttpGet("EditOrders/{orderID}")]
        public IActionResult EditOrders(string orderID)
        {
            return Ok(true);
        }
    }
}
