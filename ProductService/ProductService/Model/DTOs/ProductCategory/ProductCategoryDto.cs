using System;

namespace ProductService.Model.DTOs.ProductCategory
{
    public class ProductCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Category { get; set; }
    }
}