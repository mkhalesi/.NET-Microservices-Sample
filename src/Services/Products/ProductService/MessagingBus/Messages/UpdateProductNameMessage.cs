using System;

namespace ProductService.MessagingBus.Messages
{
    public class UpdateProductNameMessage : BaseMessage
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
    }
}
