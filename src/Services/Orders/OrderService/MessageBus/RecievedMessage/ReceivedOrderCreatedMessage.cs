using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.MessageBus.Base;
using OrderService.Model.DTOs.Basket;
using OrderService.Model.Services.RegisterOrderService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.MessageBus.RecievedMessage
{
    public class ReceivedOrderCreatedMessage : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly IRegisterOrderService registerOrderService;
        private readonly string _uri;
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _queueName;
        public ReceivedOrderCreatedMessage(IOptions<RabbitMqConfiguration> rabbitMqOptions, IRegisterOrderService regsiterOrderService)
        {
            this.registerOrderService = regsiterOrderService;
            _uri = rabbitMqOptions.Value.Uri;
            _hostName = rabbitMqOptions.Value.HostName;
            _port = rabbitMqOptions.Value.Port;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _queueName = rabbitMqOptions.Value.QueueName_BasketCheckout;

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_uri),
                //HostName = _hostName,
                //Port = _port,
                //UserName = _userName,
                //Password = _password,
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
                var basket = JsonConvert.DeserializeObject<BasketDTO>(body);

                // create order  
                var resultHandler = HandleMessage(basket);

                if (resultHandler)
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer);

            return Task.CompletedTask;
        }
        
        private bool HandleMessage(BasketDTO basket)
        {
           return registerOrderService.Execute(basket);
        }
    }
}