using System;

namespace Microservices.Web.Frontend.Models.DTO.Order
{
    public class OrderLineDto
    {
        public string Id { get; set; }
        public Guid OrderLineId => Guid.Parse(Id);
        public int Quantity { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
