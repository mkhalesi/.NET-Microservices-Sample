using System;

namespace OrderService.Model.DTOs.OrderLine
{
    public class OrderLineDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
