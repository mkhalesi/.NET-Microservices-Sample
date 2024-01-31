using MongoDB.Driver;
using OrderService.Model.Entities;
using System.Collections.Generic;

namespace OrderService.Infrastructure.Context
{
    public class OrderContextSeed
    {
        public static void SeedData(
             IMongoCollection<Order> orderCollection,
              IMongoCollection<OrderLine> orderLineCollection,
            IMongoCollection<Product> productCollection)
        {
            //if (!orderCollection.Find(p => true).Any())
            //    orderCollection.InsertManyAsync(GetPreconfiguredOrders());

            //if (!orderLineCollection.Find(p => true).Any())
            //    orderLineCollection.InsertManyAsync(GetPreconfiguredOrderLines());

            if (!productCollection.Find(p => true).Any())
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
                new Order(
                    "602d2149e773f2a3990b47f5",
                    "test",
                    "lastName",
                    "address",
                    "1212",
                    1200,
                    new List<OrderLine>
                    {
                        new OrderLine()
                        {
                            Id = "602d2149e773f2a3990b47f6",
                            Quantity = 1,
                        }
                    })
            };
        }

        //private static IEnumerable<OrderLine> GetPreconfiguredOrderLines()
        //{
        //    return new List<OrderLine>()
        //    {
        //        new OrderLine()
        //        {
        //          
        //        },
        //    };
        //}

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    ProductId = "8b23c814-da16-4fe4-9b7f-376b3a493d19",
                    Name = "IPhone X",
                    Price = 950,
                },
            };
        }
    }
}

