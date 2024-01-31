using MongoDB.Driver;
using OrderService.Model.Entities;

namespace OrderService.Infrastructure.Context
{
    public interface IOrderDataBaseContext
    {
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<OrderLine> OrderLines { get; }
        public IMongoCollection<Product> Products { get; }
    }
}
