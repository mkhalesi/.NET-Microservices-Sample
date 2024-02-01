using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Model.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRequired]
        public string ProductId { get; set; }

        [BsonRequired]
        public string Name { get; set; }

        [BsonRequired]
        public int Price { get; set; }
    }
}
