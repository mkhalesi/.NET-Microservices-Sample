using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Model.DTOs.Product;
using ProductService.Model.DTOs.ProductCategory;
using Tynamix.ObjectFiller;
using Xunit;

namespace ProductService.ComponentTests.Controllers
{
    public class ProductManagementControllerTest : IClassFixture<ProductServiceFixture>
    {
        private readonly ProductServiceFixture fixture;
        public ProductManagementControllerTest(ProductServiceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Post_NewProductInDatabase()
        {
            var newProduct = new Filler<AddNewProductDto>().Create();
            var newCategory = new Filler<CategoryDto>().Create();
            newProduct.CategoryId = fixture.categoryService.AddNewCategory(newCategory);

            var managementController =
                new ProductManagementController(fixture.productService, fixture.messageBus, fixture.options);

            var result = managementController.Post(newProduct) as CreatedResult;

            var insertedProduct = fixture.db.Products.FirstOrDefault(p => p.Id == Guid.Parse(result.Value.ToString()));
            Assert.NotNull(insertedProduct);
            Assert.Equal(newProduct.Name, insertedProduct?.Name);
        }
    }
}
