using System;
using ProductService.Model.DTOs.ProductCategory;

namespace ProductService.Model.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public ProductCategoryDto productCategory { get; set; }
    }
}