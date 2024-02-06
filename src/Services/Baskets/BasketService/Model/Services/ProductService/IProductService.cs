using System;
using BasketService.Model.DTOs.Product;

namespace BasketService.Model.Services.ProductService
{
    public interface IProductService
    {
        bool UpdateProductName(Guid productId, string productName);

        ProductDTO GetProduct(string productId);

        ProductDTO CreateProduct(ProductDTO product);
    }
}
