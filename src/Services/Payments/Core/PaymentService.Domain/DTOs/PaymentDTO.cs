using System;

namespace PaymentService.Domain.DTOs
{
    public class PaymentDTO
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public int Amount { get; set; }
        public bool IsPay { get; set; }
    }
}
