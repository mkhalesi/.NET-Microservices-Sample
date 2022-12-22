using System;
using OrderService.MessageBus.Base;

namespace OrderService.MessageBus.Messages
{
    public class UpdateProductNameMessage : BaseMessage
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
    }
}
