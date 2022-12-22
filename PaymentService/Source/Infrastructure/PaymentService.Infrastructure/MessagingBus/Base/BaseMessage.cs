using System;

namespace PaymentService.Infrastructure.MessagingBus.Base
{
    public class BaseMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}