using System;
using BasketService.Infrastructure.Contexts;

namespace BasketService.Model.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly BasketDataBaseContext context;
        public ProductService(BasketDataBaseContext context)
        {
            this.context = context;
        }
        public bool UpdateProductName(Guid productId, string productName)
        {
            var product = context.Products.Find(productId);
            product.ProductName = productName;
            context.SaveChanges();
            return true;
        }

    }
}
