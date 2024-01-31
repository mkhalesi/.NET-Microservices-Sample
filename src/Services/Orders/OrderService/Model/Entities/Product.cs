using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Model.Entities
{
    public class Product
    {
        [BsonId]
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
