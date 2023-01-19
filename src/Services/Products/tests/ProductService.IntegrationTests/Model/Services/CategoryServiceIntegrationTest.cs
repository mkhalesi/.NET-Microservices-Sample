using ProductService.Model.DTOs.ProductCategory;
using Tynamix.ObjectFiller;

namespace ProductService.IntegrationTests.Model.Services
{
    public class CategoryServiceIntegrationTest : IClassFixture<ProductServiceFixture>
    {
        private readonly ProductServiceFixture fixture;
        public CategoryServiceIntegrationTest(ProductServiceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AddNewCategoryAndGet()
        {
            //arrange
            var category = new Filler<CategoryDto>().Create();

            //act
            var categoryId = fixture.categoryService.AddNewCategory(category);
            category.Id = categoryId;
            var insertedCategory = fixture.categoryService.GetCategoryById(categoryId);

            //assert
            Assert.NotNull(insertedCategory);
            Assert.IsType<CategoryDto>(insertedCategory);
            Assert.Equal(categoryId, insertedCategory.Id);
            Assert.Equal(category.Name, insertedCategory.Name);
            Assert.Equal(category.Description, insertedCategory.Description);
        }
    }
}
