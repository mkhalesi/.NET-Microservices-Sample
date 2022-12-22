using System;

namespace Microservices.Web.Frontend.Models.DTO.Order
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int ItemCount { get; set; }
        public int TotalPrice { get; set; }
        public bool OrderPaid { get; set; }
        public DateTime OrderPlaced { get; set; }
        public PaymentStatus PaymentStatus { get; set; }


    }
}
