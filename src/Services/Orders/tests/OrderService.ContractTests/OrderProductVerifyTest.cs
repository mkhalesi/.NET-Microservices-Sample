using OrderService.Model.DTOs.Product;
using OrderService.Model.Services.ProductService;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using RestSharp;

namespace OrderService.ContractTests
{
    public class OrderProductVerifyTest : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        public OrderProductVerifyTest(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }

        [Fact]
        public void Check_Product_Verify_Api()
        {
            //Arrange
            _mockProviderService.Given("correct data")
                .UponReceiving("productName")
                .With(new ProviderServiceRequest()
                {
                    Method = HttpVerb.Get,
                    Path = "/api/product/verify/111"
                })
                .WillRespondWith(new ProviderServiceResponse()
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = Match.Type(new
                    {
                        productId = "",
                        productName = ""
                    })
                });

            //Act
            IVerifyProductService verifyProduct = new VerifyProductService(
                new RestClient(_mockProviderServiceBaseUri));

            var result = verifyProduct.Verify(new ProductDTO()
            {
                ProductId = Guid.Parse("")
            });

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Name);
        }

    }
}
