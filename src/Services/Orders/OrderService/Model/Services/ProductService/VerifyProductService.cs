using Newtonsoft.Json;
using OrderService.Model.DTOs.Product;
using RestSharp;

namespace OrderService.Model.Services.ProductService
{
    public class VerifyProductService : IVerifyProductService
    {
        private readonly RestClient restClient;
        public VerifyProductService(RestClient restClient)
        {
            this.restClient = restClient;
        }

        public VerifyProductDTO Verify(ProductDTO product)
        {
            var request = new RestRequest($"/api/product/verify/{product.ProductId}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var productOnServer = JsonConvert.DeserializeObject<ProductVerifyOnServerProductDTO>(response.Content);
            return Verify(product, productOnServer);
        }

        private VerifyProductDTO Verify(ProductDTO local, ProductVerifyOnServerProductDTO remote)
        {
            if (local.ProductName == remote.Name)
            {
                return new VerifyProductDTO() { IsCorrect = true };
            }
            else
            {
                return new VerifyProductDTO()
                {
                    IsCorrect = false,
                    Name = remote.Name
                };
            }
        }
    }
}