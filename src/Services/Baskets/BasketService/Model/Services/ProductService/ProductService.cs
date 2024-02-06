using System;
using System.Linq;
using AutoMapper;
using BasketService.Infrastructure.Contexts;
using BasketService.Model.DTOs.Product;
using BasketService.Model.Entities;
using BasketService.Model.Services.CacheService;

namespace BasketService.Model.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly BasketDataBaseContext context;
        private readonly ICacheService _cacheService;
        private readonly IMapper mapper;
        public ProductService(BasketDataBaseContext context, ICacheService cacheService, IMapper mapper)
        {
            this.context = context;
            _cacheService = cacheService;
            this.mapper = mapper;
        }

        public bool UpdateProductName(Guid productId, string productName)
        {
            var product = context.Products.Find(productId);
            product.ProductName = productName;
            context.SaveChanges();
            return true;
        }

        public ProductDTO GetProduct(string productId)
        {
            //var product = _cacheService.Get<Product>(productId);
            var existsProduct = context.Products.FirstOrDefault(p => p.ProductId == Guid.Parse(productId));
            if (existsProduct == null)
                return null;

            return mapper.Map<ProductDTO>(existsProduct);
        }

        public ProductDTO CreateProduct(ProductDTO product)
        {
            var existsProduct = GetProduct(product.ProductId);
            if (existsProduct != null)
                return existsProduct;

            var newProduct = mapper.Map<Product>(product);
            context.Products.Add(newProduct);
            context.SaveChanges();

            //var newProduct = _cacheService.Set<Product>(product.ProductId, mapper.Map<Product>(product));

            return mapper.Map<ProductDTO>(newProduct);
        }

    }
}
