using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Model.DTOs.Product;
using ProductService.Model.DTOs.ProductCategory;
using Tynamix.ObjectFiller;

namespace ProductService.IntegrationTests.Controllers
{
    public class ProductManagementControllerIntegrationTest : IClassFixture<ProductServiceFixture>
    {
        private readonly ProductServiceFixture fixture;
        public ProductManagementControllerIntegrationTest(ProductServiceFixture fixture)
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
            var addedId = Guid.Parse(result.Value.ToString());

            var insertedProduct = fixture.productService.GetProduct(addedId);
            Assert.NotNull(insertedProduct);
            Assert.Equal(addedId, insertedProduct.Id);
            Assert.Equal(newProduct.CategoryId, insertedProduct.productCategory.CategoryId);
            Assert.Equal(newProduct.Name, insertedProduct?.Name);
            Assert.Equal(newProduct.Description, insertedProduct.Description);
            Assert.Equal(newProduct.Image, insertedProduct.Image);
            Assert.Equal(newProduct.Price, insertedProduct.Price);
        }
    }
}
