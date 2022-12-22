using System;
using PaymentService.Infrastructure.MessagingBus.Base;

namespace PaymentService.Infrastructure.MessagingBus.Messages
{
    public class PaymentIsDoneMessage : BaseMessage
    {
        public Guid OrderId { get; set; }
    }
}
