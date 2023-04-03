using Microservices.Admin.Frontend.Models.Dto;

namespace Microservices.Admin.Frontend.Models.ViewServices.ProductServices;

public interface IProductManagementService
{
    List<ProductDto>? GetProducts();
    ResultDto UpdateName(UpdateProductDto updateProduct);

}