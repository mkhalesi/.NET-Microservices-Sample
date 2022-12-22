using System;

namespace OrderService.Model.DTOs.Product
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
    }
}
