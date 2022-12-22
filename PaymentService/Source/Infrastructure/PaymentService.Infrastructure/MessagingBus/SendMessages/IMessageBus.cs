using PaymentService.Infrastructure.MessagingBus.Base;

namespace PaymentService.Infrastructure.MessagingBus.SendMessages { 
    public interface IMessageBus  {
         void SendMessage(BaseMessage message, string QueueName);
    }
}