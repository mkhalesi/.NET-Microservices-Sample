using System;
using System.Net;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public class BasketServices : IBasketService
    {
        #region constructor

        private RestClient restClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasketServices(RestClient clientRest, IHttpContextAccessor httpContextAccessor)
        {
            this.restClient = clientRest;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        // change for OriginAzure
        public async Task<BasketDTO> GetBasket(string userId)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var request = new RestRequest($"/api/Basket?userId={userId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await restClient.ExecuteAsync(request);

            //var basket = JsonSerializer.Deserialize<BasketDTO>(response.Content);
            var basket = JsonConvert.DeserializeObject<BasketDTO>(response.Content);
            return basket;
        }

        public async Task<ResultDTO> AddToBasket(AddToBasketDTO addToBasket, string userId)
        {
            var request = new RestRequest($"/api/Basket?userId={userId}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            string serializeModel = JsonSerializer.Serialize(addToBasket);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            var response = await restClient.ExecuteAsync(request);
            return GetResponseStatusCode(response);
        }

        public async Task<ResultDTO> DeleteFromBasket(Guid Id, string userId)
        {
            var request = new RestRequest($"/api/Basket?ItemId={Id}&userId={userId}", Method.DELETE);
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = restClient.Execute(request);
            return GetResponseStatusCode(response);
        }

        public async Task<ResultDTO> UpdateQuantity(Guid basketItemId, int quantity, string userId)
        {
            var request = new RestRequest($"/api/Basket?basketItemId={basketItemId}&quantity={quantity}&userId={userId}", Method.PUT);
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await restClient.ExecuteAsync(request);
            return GetResponseStatusCode(response);
        }

        public async Task<ResultDTO> ApplyDiscountToBasket(Guid basketId, Guid discountId, string userId)
        {
            //https://localhost:6001/api/Basket/7d9df6bc-8e91-476f-c28d-08d983442ffa/9d9df6bc-8e91-476f-c28d-08d983442ffa
            var request = new RestRequest($"/api/Basket/{basketId}/{discountId}/{userId}", Method.PUT);
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await restClient.ExecuteAsync(request);
            return GetResponseStatusCode(response);
        }

        public async Task<ResultDTO> Checkout(CheckoutDTO checkout)
        {
            var request = new RestRequest($"api/Basket/CheckoutBasket", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var serializeModel = JsonSerializer.Serialize(checkout);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await restClient.ExecuteAsync(request);
            return GetResponseStatusCode(response);
        }

        #region Helper methods

        private static ResultDTO GetResponseStatusCode(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
                return new ResultDTO() { IsSuccess = true };

            return new ResultDTO()
            {
                IsSuccess = false,
                ErrorMessage = response.ErrorMessage
            };
        }

        #endregion
    }
}