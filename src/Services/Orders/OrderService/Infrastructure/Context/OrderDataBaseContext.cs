using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderService.Model.Entities;

namespace OrderService.Infrastructure.Context
{
    public class OrderDataBaseContext : IOrderDataBaseContext
    {
        public OrderDataBaseContext(IConfiguration configuration)
        {
            //var connectionString = $"{configuration["DatabaseSettings:ConnectionString"]}/${configuration["DatabaseSettings:DatabaseName"]}";
            //var mongoUrl = new MongoUrl(connectionString);
            //var client = new MongoClient(mongoUrl.Url);
            //var database = client.GetDatabase(mongoUrl.DatabaseName);

            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

            Orders = database.GetCollection<Order>("Orders");
            OrderLines = database.GetCollection<OrderLine>("OrderLines");
            Products = database.GetCollection<Product>("Products");

            OrderContextSeed.SeedData(Orders, OrderLines, Products);
        }

        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<OrderLine> OrderLines { get; }
        public IMongoCollection<Product> Products { get; }
    }
}