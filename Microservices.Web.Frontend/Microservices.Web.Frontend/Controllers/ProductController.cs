using System;
using Microservices.Web.Frontend.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Web.Frontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public IActionResult Index()
        {
            var products = productService.GetAllProduct();
            return View(products);
        }

        public IActionResult Details(Guid id)
        {
            var product = productService.GetProduct(id);
            return View(product);
        }
    }
}
