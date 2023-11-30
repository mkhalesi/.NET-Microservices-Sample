using System;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Microservices.Web.Frontend.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly RestClient restClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentService(RestClient restClient, IHttpContextAccessor httpContextAccessor)
        {
            this.restClient = restClient;
            _httpContextAccessor = httpContextAccessor;
            restClient.Timeout = -1;
        }

        public async Task<ResultDTO<ReturnPaymentLinkDTO>> GetPaymentLink(Guid OrderId, string CallbackUrl)
        {
            var request = new RestRequest($"/api/Pay?OrderId={OrderId}&callbackUrlFront={CallbackUrl}", Method.GET);
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.AddHeader("Authorization", $"Bearer {token}");

            IRestResponse response = await restClient.ExecuteAsync(request);
            var orders = JsonConvert.DeserializeObject<ResultDTO<ReturnPaymentLinkDTO>>(response.Content);
            return orders;
        }
    }
}