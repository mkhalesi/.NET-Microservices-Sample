using System;
using MongoDB.Driver;
using OrderService.Infrastructure.Context;
using OrderService.Model.DTOs.Product;
using OrderService.Model.Entities;

namespace OrderService.Model.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IOrderDataBaseContext context;
        public ProductService(IOrderDataBaseContext context)
        {
            this.context = context;
        }

        public Product GetProduct(ProductDTO product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, product.ProductId);
            var existProduct = context.Products.Find(filter).FirstOrDefault();
            if (existProduct != null)
                return existProduct;

            return CreateNewProduct(product);
        }

        public bool UpdateProductName(string productId, string productName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            var product = context.Products.Find(filter).FirstOrDefault();

            if (product == null)
                return false;

            product.Name = productName;
            var updateRes = context.Products.ReplaceOne(filter, product);

            return updateRes.IsAcknowledged && updateRes.ModifiedCount > 0;
        }

        private Product CreateNewProduct(ProductDTO productDTO)
        {
            Product product = new Product
            {
                ProductId = productDTO.ProductId,
                Name = productDTO.ProductName,
                Price = productDTO.ProductPrice
            };

            context.Products.InsertOne(product);

            return product;
        }
    }
}