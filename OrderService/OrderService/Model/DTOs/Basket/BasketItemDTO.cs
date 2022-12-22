using System;

namespace OrderService.Model.DTOs.Basket
{
    public class BasketItemDTO
    {
        public Guid BasketItemId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
