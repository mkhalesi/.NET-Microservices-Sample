using Microservices.Admin.Frontend.Models.ViewServices.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Admin.Frontend.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManagementService productManagementService;

        public ProductController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService;
        }
        public IActionResult Index()
        {
            return View(productManagementService.GetProducts());
        }

        [HttpPost]
        public IActionResult UpdateName(Guid ProductId, string Name)
        {
            var result = productManagementService
                .UpdateName(new
                UpdateProductDto(ProductId, Name));
            return RedirectToAction(nameof(Index));
        }
    }
}
