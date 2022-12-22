using System;
using PaymentService.Infrastructure.MessagingBus.Base;

namespace PaymentService.Infrastructure.MessagingBus.Messages
{
    public class PaymentMessageDTO : BaseMessage
    {
        public Guid OrderId { get; set; }
        public int Amount { get; set; }
    }
}
