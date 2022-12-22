using System;

namespace BasketService.Model.DTOs.MessageDTO
{
    public class BasketItemMessage
    {
        public Guid BasketItemId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
