using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.MessageBus.Base;
using OrderService.Model.DTOs.Messages;
using OrderService.Model.Services.OrderService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.MessageBus.RecievedMessage
{
    public class ReceivedPaymentOfOrderMessage: BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IOrderService orderService;
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _queueName;
        public ReceivedPaymentOfOrderMessage(IOptions<RabbitMqConfiguration> rabbitMqOptions, 
            IOrderService orderService)
        {
            this.orderService = orderService;
            _hostName = rabbitMqOptions.Value.HostName;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _queueName = rabbitMqOptions.Value.QueueName_PaymentDone;

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
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
                var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var paymentDone = JsonConvert.DeserializeObject<PaymentOrderMessage>(body);

                var resultHandler = HandleMessage(paymentDone);

                if (resultHandler)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer);

            return Task.CompletedTask;
        }

        private bool HandleMessage(PaymentOrderMessage payment)
        {
            return orderService.PaymentIsDoneOrder(payment.OrderId);
        }
    }
}
