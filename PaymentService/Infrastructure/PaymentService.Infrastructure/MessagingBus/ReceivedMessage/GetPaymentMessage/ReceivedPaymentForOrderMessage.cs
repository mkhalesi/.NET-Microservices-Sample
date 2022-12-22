using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentService.Application.Services;
using PaymentService.Infrastructure.MessagingBus.Base;
using PaymentService.Infrastructure.MessagingBus.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentService.Infrastructure.MessagingBus.ReceivedMessage.GetPaymentMessage
{
    public class ReceivedPaymentForOrderMessage : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IPaymentService paymentService;
        private readonly string _hostName;
        private readonly string _quequeName;
        private readonly string _userName;
        private readonly string _password;
        public ReceivedPaymentForOrderMessage(IPaymentService paymentService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.paymentService = paymentService;
            _hostName = rabbitMqOptions.Value.HostName;
            _quequeName = rabbitMqOptions.Value.QueueName_OrderSendToPayment;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _quequeName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var orderToPayment = JsonConvert.DeserializeObject<PaymentMessageDTO>(content);

                // create payment
                var resultHandler = HandleMessage(orderToPayment);

                if (resultHandler)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            _channel.BasicConsume(queue: _quequeName, autoAck: false, consumer);

            return Task.CompletedTask;
        }

        private bool HandleMessage(PaymentMessageDTO payment)
        {
            return paymentService.CreatePayment(payment.OrderId, payment.Amount);
        }
    }
}
