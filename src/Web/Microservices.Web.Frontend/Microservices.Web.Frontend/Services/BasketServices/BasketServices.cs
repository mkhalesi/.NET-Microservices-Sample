using System;
using System.Net;
using System.Text.Json;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;
using RestSharp;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public class BasketServices : IBasketService
    {
        #region constructor

        private RestClient restClient;
        public BasketServices(RestClient clientRest)
        {
            this.restClient = clientRest;
            restClient.Timeout = -1;
        }

        #endregion

        public BasketDTO GetBasket(string userId)
        {
            var request = new RestRequest($"/api/Basket?UserId={userId}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var basket = JsonSerializer.Deserialize<BasketDTO>(response.Content);
            return basket;
        }

        public ResultDTO AddToBasket(AddToBasketDTO addToBasket, string UserId)
        {
            var request = new RestRequest($"/api/Basket?UserId={UserId}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            string serializeModel = JsonSerializer.Serialize(addToBasket);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            var response = restClient.Execute(request);
            return getResponseStatusCode(response);
        }

        public ResultDTO DeleteFromBasket(Guid Id)
        {
            var request = new RestRequest($"/api/Basket?ItemId=${Id}", Method.DELETE);
            IRestResponse response = restClient.Execute(request);
            return getResponseStatusCode(response);
        }

        public ResultDTO UpdateQuantity(Guid basketItemId, int quantity)
        {
            var request = new RestRequest($"/api/Basket?basketItemId=${basketItemId}&quantity={quantity}", Method.PUT);
            IRestResponse response = restClient.Execute(request);
            return getResponseStatusCode(response);
        }

        public ResultDTO ApplyDiscountToBasket(Guid basketId, Guid discountId)
        {
            //https://localhost:6001/api/Basket/7d9df6bc-8e91-476f-c28d-08d983442ffa/9d9df6bc-8e91-476f-c28d-08d983442ffa
            var request = new RestRequest($"/api/Basket/{basketId}/{discountId}", Method.PUT);
            IRestResponse response = restClient.Execute(request);
            return getResponseStatusCode(response);
        }

        public ResultDTO Checkout(CheckoutDTO checkout)
        {
            var request = new RestRequest($"api/Basket/CheckoutBasket", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var serializeModel = JsonSerializer.Serialize(checkout);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            return getResponseStatusCode(response);
        }

        #region Helper methods

        private static ResultDTO getResponseStatusCode(IRestResponse response)
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