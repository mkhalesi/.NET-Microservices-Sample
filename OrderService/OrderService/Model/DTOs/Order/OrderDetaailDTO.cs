using System;
using System.Collections.Generic;
using OrderService.Model.DTOs.OrderLine;
using OrderService.Model.Entities;

namespace OrderService.Model.DTOs.Order
{
    public class orderDetailDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderPlaced { get; set; }
        public bool OrderPaid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderLineDto> OrderLines { get; set; }
    }
}
