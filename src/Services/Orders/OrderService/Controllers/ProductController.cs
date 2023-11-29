using System;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.DTOs.Product;
using OrderService.Model.Services.ProductService;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IVerifyProductService _verifyProductService;
        private IProductService _productService;
        public ProductController(IVerifyProductService verifyProductService,
            IProductService productService)
        {
            _verifyProductService = verifyProductService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Verify(Guid Id)
        {
            var product = _productService.GetProduct(new ProductDTO() { ProductId = Id });
            return Ok(_verifyProductService.Verify(new ProductDTO()
            {
                ProductId = Id,
                ProductName = product.Name
            }));
        }

    }
}
