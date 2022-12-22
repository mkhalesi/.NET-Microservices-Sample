using System;
using System.Collections.Generic;
using OrderService.Model.DTOs.OrderLine;
using OrderService.Model.Entities;

namespace OrderService.Model.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int ItemCount { get; set; }
        public int TotalPrice { get; set; }
        public bool OrderPaid { get; set; }
        public DateTime OrderPlaced { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderLineDto> OrderLines { get; set; }
    }
}
