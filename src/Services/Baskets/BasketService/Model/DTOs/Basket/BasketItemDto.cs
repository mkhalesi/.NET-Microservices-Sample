using System;

namespace BasketService.Model.DTOs.Basket
{
    public class BasketItemDto
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string ProductId { get; set; }
        public Entities.Product Product { get; set; }
    }
}