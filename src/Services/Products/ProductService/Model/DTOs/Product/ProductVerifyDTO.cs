using System;

namespace ProductService.Model.DTOs.Product
{
    public class ProductVerifyDTO
    {
        public ProductVerifyDTO(ProductDto product)
        {
            Id = product.Id;
            Name = product.Name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
