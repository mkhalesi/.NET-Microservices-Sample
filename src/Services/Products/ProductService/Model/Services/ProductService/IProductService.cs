using System;
using System.Collections.Generic;
using ProductService.Model.DTOs.Product;

namespace ProductService.Model.Services.ProductService
{
    public interface IProductService
    {
        List<ProductDto> GetProductList();
        ProductDto GetProduct(Guid Id);
        Guid AddNewProduct(AddNewProductDto addNewProduct);
        bool UpdateProductName(UpdateProductDto updateProduct);
    }
}
