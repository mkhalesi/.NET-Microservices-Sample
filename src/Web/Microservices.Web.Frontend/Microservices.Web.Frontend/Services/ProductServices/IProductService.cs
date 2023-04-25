using Microservices.Web.Frontend.Models.DTO.Product;
using System;
using System.Collections.Generic;

namespace Microservices.Web.Frontend.Services.ProductServices
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProduct();
        ProductDto GetProduct(Guid Id);
    }
}
