using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Order;
using Newtonsoft.Json;
using RestSharp;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly RestClient restClient;
        private string _accessToken = null;
        public OrderService(RestClient restClient)
        {
            this.restClient = restClient;
            restClient.Timeout = -1;
        }

        private async Task<string> GetAccessToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
                return _accessToken;

            HttpClient client = new HttpClient();
            var discoveryDocument = await client.GetDiscoveryDocumentAsync("https://localhost:7017");
            var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "webFrontend",
                ClientSecret = "123321",
                Scope = "OrderService.FullAccess",
                GrantType = OidcConstants.GrantTypes.ClientCredentials
            });

            if (token.IsError)
                throw new Exception(token.ErrorDescription);

            _accessToken = token.AccessToken;

            return _accessToken;
        }

        public async Task<List<OrderDTO>> GetOrders(string UserId)
        {
            var request = new RestRequest("/api/Order", Method.GET);
            request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
            IRestResponse response = await restClient.ExecuteAsync(request);
            var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);
            return orders;
        }

        public async Task<OrderDetailDTO> OrderDetail(Guid OrderId)
        {
            var request = new RestRequest($"/api/Order/{OrderId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
            IRestResponse response = await restClient.ExecuteAsync(request);
            var orderdetail = JsonConvert.DeserializeObject<OrderDetailDTO>(response.Content);
            return orderdetail;
        }

        public async Task<ResultDTO> RequestPayment(Guid OrderId)
        {
            var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.POST);
            request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = await restClient.ExecuteAsync(request);
            return GetResponseStatusCode(response);
        }

        private static ResultDTO GetResponseStatusCode(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new ResultDTO
                {
                    IsSuccess = true,
                };
            }
            else
            {
                return new ResultDTO
                {
                    IsSuccess = false,
                    Message = response.ErrorMessage
                };
            }
        }
    }
}