using System;

namespace BasketService.Model.Services.ProductService
{
    public interface IProductService
    {
        bool UpdateProductName(Guid productId, string productName);
    }
}
