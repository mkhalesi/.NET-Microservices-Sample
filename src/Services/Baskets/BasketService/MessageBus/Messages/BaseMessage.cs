using System;

namespace BasketService.MessageBus.Messages
{
    public class BaseMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}