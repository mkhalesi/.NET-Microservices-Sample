using Microservices.Web.Frontend.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO.Order;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetOrders(string UserId);
        Task<OrderDetailDTO> OrderDetail(Guid OrderId);
        Task<ResultDTO> RequestPayment(Guid OrderId);

    }
}
