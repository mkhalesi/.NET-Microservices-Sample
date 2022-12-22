using System;

namespace ProductService.Model.DTOs.Product
{
    public class AddNewProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}