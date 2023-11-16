using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductService.MessagingBus.Config;
using ProductService.MessagingBus.Messages;
using RabbitMQ.Client;

namespace ProductService.MessagingBus.SendMessages
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly string _uri;
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMqMessageBus(IOptions<RabbitMqConfiguration> options)
        {
            _uri = options.Value.Uri;
            //_hostName = options.Value.HostName;
            //Port = _port,
            //_userName = options.Value.UserName;
            //_password = options.Value.Password;

            CreateRabbitMqConnection();
        }

        public void SendMessage(BaseMessage message, string exchange)
        {
            if (!CheckRabbitMqConnection())
                return;

            using var channel = _connection.CreateModel();
            //channel.QueueDeclare(queue: QueueName, 
            //    durable: true, 
            //    exclusive: false,
            //    autoDelete:false,
            //    arguments: null
            //    );

            channel.ExchangeDeclare(exchange , ExchangeType.Fanout , true , false , null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            //channel.BasicPublish(exchange: "" ,
            //    routingKey: QueueName ,
            //    basicProperties: properties ,
            //    body: body);
            channel.BasicPublish(exchange: exchange,
                routingKey:"",
                basicProperties: properties,
                body: body);
        }

        private void CreateRabbitMqConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_uri),
                    //HostName = _hostName,
                    //UserName = _userName,
                    //Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"can not create connection: {ex.Message}");
            }
        }

        private bool CheckRabbitMqConnection()
        {
            if (_connection != null)
                return true;

            CreateRabbitMqConnection();
            return _connection != null;
        }

    }
}
