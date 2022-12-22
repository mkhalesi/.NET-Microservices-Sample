namespace PaymentService.Infrastructure.MessagingBus.Base
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public string QueueName_OrderSendToPayment { get; set; }
        public string QueueName_PaymentDone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
