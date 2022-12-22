using System;
using System.Linq;
using OrderService.Infrastructure.Context;
using OrderService.Model.DTOs.Product;
using OrderService.Model.Entities;

namespace OrderService.Model.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly OrderDataBaseContext context;
        public ProductService(OrderDataBaseContext context)
        {
            this.context = context;
        }

        public Product GetProduct(ProductDTO product)
        {
            var existProduct = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existProduct != null)
                return existProduct;

            return CreateNewProduct(product);
        }

        public bool UpdateProductName(Guid productId, string productName)
        {
            var product = context.Products.Find(productId);
            product.Name = productName;
            context.SaveChanges();
            return true;
        }

        private Product CreateNewProduct(ProductDTO productDTO)
        {
            Product product = new Product
            {
                ProductId = productDTO.ProductId,
                Name = productDTO.ProductName,
                Price = productDTO.ProductPrice
            };
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }
    }
}