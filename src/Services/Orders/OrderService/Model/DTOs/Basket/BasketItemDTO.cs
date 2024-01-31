using System;

namespace OrderService.Model.DTOs.Basket
{
    public class BasketItemDTO
    {
        public string BasketItemId { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
