namespace ProductService.MessagingBus.Config
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeName_UpdateProductName { get; set; }
    }
}
