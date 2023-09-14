using Microsoft.AspNetCore.Mvc;
using ProductService.Model.DTOs.Product;
using ProductService.Model.Services.ProductService;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAdminController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductAdminController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddNewProductDto addNewProductDto)
        {
            productService.AddNewProduct(addNewProductDto);
            return Ok();
        }
    }
}
