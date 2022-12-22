using BasketService.MessageBus.Messages;

namespace BasketService.MessageBus.SendMessages
{
    public interface IMessageBus
    {
        void SendMessage(BaseMessage message, string QueueName);
    }
}
