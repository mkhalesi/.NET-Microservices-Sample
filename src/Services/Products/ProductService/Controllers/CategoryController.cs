using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model.DTOs.ProductCategory;
using ProductService.Model.Services.ProductCategoryService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var data = categoryService.GetCategories();
            return Ok(data);
        }

        // POST api/<CategoryController>
        [Authorize(Policy = "ProductsManagement")]
        [HttpPost]
        public IActionResult Post([FromBody] CategoryDto  categoryDto)
        {
            categoryService.AddNewCategory(categoryDto);
            return Ok();
        }
 
    }
}
