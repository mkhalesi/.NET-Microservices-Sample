using ProductService.MessagingBus.Messages;

namespace ProductService.MessagingBus.SendMessages
{
    public interface IMessageBus
    {
        void SendMessage(BaseMessage message, string QueueName);
    }
}