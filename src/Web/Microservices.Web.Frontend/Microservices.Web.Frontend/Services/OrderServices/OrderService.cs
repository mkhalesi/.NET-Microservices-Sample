using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Order;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            this._httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<OrderDTO>> GetOrders(string UserId)
        {
            //get access token & refers token
            var accessToken = await _httpContextAccessor?.HttpContext.GetTokenAsync("access_token");
            var refersToken = await _httpContextAccessor.HttpContext.GetTokenAsync("refers_token");

            var response = await _httpClient.GetAsync("/api/Order");
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(json);
            return orders;
        }

        public async Task<OrderDetailDTO> OrderDetail(Guid OrderId)
        {
            var response = await _httpClient.GetAsync($"/api/Order/{OrderId}");
            var json = await response.Content.ReadAsStringAsync();
            var orderDetail = JsonConvert.DeserializeObject<OrderDetailDTO>(json);
            return orderDetail;
        }

        public async Task<ResultDTO> RequestPayment(Guid OrderId)
        {
            //var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.POST);
            //request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
            //request.AddHeader("Content-Type", "application/json");
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

            var httpResponseMessage = await _httpClient.PostAsync($"/api/OrderPayment?OrderId={OrderId}", null);
            return GetResponseStatusCode(httpResponseMessage);
        }

        private static ResultDTO GetResponseStatusCode(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
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
                    //Message = response.Content
                };
            }
        }

        #region using RestClient

        //private readonly RestClient restClient;
        //private readonly IHttpContextAccessor _contextAccessor;
        //private string _accessToken = null;
        //public OrderService(RestClient restClient, IHttpContextAccessor contextAccessor)
        //{
        //    this.restClient = restClient;
        //    _contextAccessor = contextAccessor;
        //    restClient.Timeout = -1;
        //}

        //private async Task<string> GetAccessToken()
        //{
        //    if (!string.IsNullOrWhiteSpace(_accessToken))
        //        return _accessToken;

        //    //HttpClient client = new HttpClient();
        //    //var discoveryDocument = await client.GetDiscoveryDocumentAsync("https://localhost:7017");
        //    //var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        //    //{
        //    //    Address = discoveryDocument.TokenEndpoint,
        //    //    ClientId = "webFrontend",
        //    //    ClientSecret = "123321",
        //    //    Scope = "OrderService.FullAccess",
        //    //    GrantType = OidcConstants.GrantTypes.ClientCredentials
        //    //});

        //    //if (token.IsError)
        //    //    throw new Exception(token.ErrorDescription);

        //    //_accessToken = token.AccessToken;

        //    _accessToken = await _contextAccessor.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

        //    return _accessToken;
        //}

        //public async Task<List<OrderDTO>> GetOrders(string UserId)
        //{
        //    var request = new RestRequest("/api/Order", Method.GET);
        //    request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
        //    IRestResponse response = await restClient.ExecuteAsync(request);
        //    var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);
        //    return orders;
        //}

        //public async Task<OrderDetailDTO> OrderDetail(Guid OrderId)
        //{
        //    var request = new RestRequest($"/api/Order/{OrderId}", Method.GET);
        //    request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
        //    IRestResponse response = await restClient.ExecuteAsync(request);
        //    var orderdetail = JsonConvert.DeserializeObject<OrderDetailDTO>(response.Content);
        //    return orderdetail;
        //}

        //public async Task<ResultDTO> RequestPayment(Guid OrderId)
        //{
        //    var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.POST);
        //    request.AddHeader("Authorization", $"Bearer {await GetAccessToken()}");
        //    request.AddHeader("Content-Type", "application/json");
        //    IRestResponse response = await restClient.ExecuteAsync(request);
        //    return GetResponseStatusCode(response);
        //}

        //private static ResultDTO GetResponseStatusCode(IRestResponse response)
        //{
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        return new ResultDTO
        //        {
        //            IsSuccess = true,
        //        };
        //    }
        //    else
        //    {
        //        return new ResultDTO
        //        {
        //            IsSuccess = false,
        //            Message = response.ErrorMessage
        //        };
        //    }
        //}

        #endregion
    }
}