using System;

namespace BasketService.MessageBus.Messages
{
    public class UpdateProductNameMessage : BaseMessage
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
    }
}
