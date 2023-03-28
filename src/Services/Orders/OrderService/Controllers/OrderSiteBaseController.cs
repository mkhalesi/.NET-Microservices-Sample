using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "GetOrders")]
    public class OrderSiteBaseController : ControllerBase
    {
    }
}
