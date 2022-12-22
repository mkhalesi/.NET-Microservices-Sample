using System;
using OrderService.Model.DTOs.Product;
using OrderService.Model.Entities;

namespace OrderService.Model.Services.ProductService
{
    public interface IProductService
    {
        Product GetProduct(ProductDTO product);

        bool UpdateProductName(Guid productId, string productName);
    }
}