using PactNet;
using PactNet.Mocks.MockHttpService;

namespace OrderService.ContractTests
{
    public class ConsumerPactClassFixture : IDisposable
    {
        public IPactBuilder PactBuilder { get; set; }
        public IMockProviderService MockProviderService { get; set; }
        public int MockServerPort => 5001;
        public string MockProviderServiceBaseUri => $"https://localhost:{MockServerPort}";
        public ConsumerPactClassFixture()
        {
            var pactConfig = new PactConfig()
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"c:\pacts",
                LogDir = @".\pact_logs"
            };
            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer("OrderServiceConsumer")
                .HasPactWith("productServiceProvider");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
