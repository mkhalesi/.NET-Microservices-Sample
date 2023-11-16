namespace ProductService.MessagingBus.Config
{
    public class RabbitMqConfiguration
    {
        public string Uri { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName_UpdateProductName { get; set; }
    }
}
