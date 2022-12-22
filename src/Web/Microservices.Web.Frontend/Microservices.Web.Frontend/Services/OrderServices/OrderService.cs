using System;
using System.Collections.Generic;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Order;
using Newtonsoft.Json;
using RestSharp;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly RestClient restClient;
        public OrderService(RestClient restClient)
        {
            this.restClient = restClient;
            restClient.Timeout = -1;
        }

        public List<OrderDTO> GetOrders(string UserId)
        {
            var request = new RestRequest("/api/Order", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Content);
            return orders;
        }

        public OrderDetailDTO OrderDetail(Guid OrderId)
        {
            var request = new RestRequest($"/api/Order/{OrderId}", Method.GET);
            IRestResponse response = restClient.Execute(request);
            var orderdetail = JsonConvert.DeserializeObject<OrderDetailDTO>(response.Content);
            return orderdetail;
        }

        public ResultDTO RequestPayment(Guid OrderId)
        {
            var request = new RestRequest($"/api/OrderPayment?OrderId={OrderId}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = restClient.Execute(request);
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