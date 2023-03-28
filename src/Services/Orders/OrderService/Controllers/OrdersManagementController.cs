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
        public IActionResult EditOrders(Guid orderID)
        {
            return Ok(true);
        }
    }
}
