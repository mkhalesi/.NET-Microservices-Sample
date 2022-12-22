using System;

namespace PaymentService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
}
