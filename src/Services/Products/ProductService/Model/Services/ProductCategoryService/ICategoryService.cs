using System.Collections.Generic;
using ProductService.Model.DTOs.ProductCategory;

namespace ProductService.Model.Services.ProductCategoryService
{
    public interface ICategoryService
    {
        List<CategoryDto> GetCategories();
        void AddNewCatrgory(CategoryDto category);
    }
}
