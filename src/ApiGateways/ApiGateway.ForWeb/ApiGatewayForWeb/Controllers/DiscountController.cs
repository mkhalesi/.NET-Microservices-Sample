using ApiGateway.ForWeb.Models.DiscountServices;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.ForWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService discountService;

        public DiscountController(IDiscountService discountService)
        {
            this.discountService = discountService;
        }

        [HttpGet]
        public IActionResult Get(string Code)
        {
            var result = discountService.GetDiscountByCode(Code);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(Guid Id)
        {
            var result = discountService.GetDiscountById(Id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put(Guid Id)
        {
            var result = discountService.UseDiscount(Id);
            return Ok(result);
        }
    }
}
