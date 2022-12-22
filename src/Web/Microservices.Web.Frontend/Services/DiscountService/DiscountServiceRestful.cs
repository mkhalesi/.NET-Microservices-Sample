using System;
using System.Net;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Discount;
using Newtonsoft.Json;
using RestSharp;

namespace Microservices.Web.Frontend.Services.DiscountService
{
    public class DiscountServiceRestful : IDiscountService
    {
        private readonly RestClient restClient;
        public DiscountServiceRestful(RestClient restrtClient)
        {
            this.restClient = restrtClient;
        }

        public ResultDTO<DiscountDTO> GetDiscountByCode(string code)
        {
            var request = new RestRequest($"/api/Discount?code={code}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var discount = JsonConvert.DeserializeObject<ResultDTO<DiscountDTO>>(response.Content);
            return discount;
        }

        public ResultDTO<DiscountDTO> GetDiscountById(Guid Id)
        {
            var request = new RestRequest($"/api/Discount?Id={Id}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var discount = JsonConvert.DeserializeObject<ResultDTO<DiscountDTO>>(response.Content);
            return discount;
        }

        public ResultDTO UseDiscount(Guid DiscountId)
        {
            var request = new RestRequest($"/api/Discount/Put?Id{DiscountId}", Method.PUT);
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
