
using OrderService.MessageBus.Base;

namespace OrderService.MessageBus.SendMessages { 
    public interface IMessageBus  {
         void SendMessage(BaseMessage message, string QueueName);
    }
}