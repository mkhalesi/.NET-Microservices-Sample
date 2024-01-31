using System;
using System.Collections.Generic;

namespace Microservices.Web.Frontend.Models.DTO.Order
{
    public class OrderDetailDTO
    {
        public string Id { get; set; }
        public Guid OrderId => Guid.Parse(Id);
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
