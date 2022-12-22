using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.MessageBus.Base;
using OrderService.MessageBus.Messages;
using OrderService.Model.Services.ProductService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.MessageBus.RecievedMessage.ProductMessages
{
    public class ReceivedUpdateProductNameMessage : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IProductService productService;
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _exchangeName;
        public ReceivedUpdateProductNameMessage(IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IProductService productService)
        {
            this.productService = productService;
            _hostName = rabbitMqOptions.Value.HostName;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _exchangeName = rabbitMqOptions.Value.ExchangeName_UpdateProductName;
            _queueName = rabbitMqOptions.Value.QueueName_GetMessageOnUpdateProductName;

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true, false, null);
            _channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.QueueBind(_queueName, _exchangeName, "", null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var inCommingProduct = JsonConvert.DeserializeObject<UpdateProductNameMessage>(body);

                var resultHandler = HandleMessage(inCommingProduct);

                if (resultHandler)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer);

            return Task.CompletedTask;
        }

        private bool HandleMessage(UpdateProductNameMessage inCommingProduct)
        {
            return productService.UpdateProductName(inCommingProduct.Id, inCommingProduct.NewName);
        }

    }
}
