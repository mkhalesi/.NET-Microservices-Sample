using System;

namespace Microservices.Web.Frontend.Models.DTO.Order
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public Guid OrderId => Guid.Parse(Id);
        public int ItemCount { get; set; }
        public int TotalPrice { get; set; }
        public bool OrderPaid { get; set; }
        public DateTime OrderPlaced { get; set; }
        public PaymentStatus PaymentStatus { get; set; }


    }
}
