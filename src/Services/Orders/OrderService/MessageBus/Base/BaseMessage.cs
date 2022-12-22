using System;

namespace OrderService.MessageBus.Base
{
    public class BaseMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}