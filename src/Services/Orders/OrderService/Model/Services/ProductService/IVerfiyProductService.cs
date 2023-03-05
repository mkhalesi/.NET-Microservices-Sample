using OrderService.Model.DTOs.Product;

namespace OrderService.Model.Services.ProductService
{
    public interface IVerifyProductService
    {
        VerifyProductDTO Verify(ProductDTO product);
    }
}
