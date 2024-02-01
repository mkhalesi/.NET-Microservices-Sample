using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OrderService.Model.Entities
{
    public class OrderLine
    {
        [BsonId]
        [BsonRequired]
        public string Id { get; set; }
        //public Guid OrderId { get; set; }
        //public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        //[ForeignKey("OrderId")]
        public Order Order { get; set; }

        //[ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}