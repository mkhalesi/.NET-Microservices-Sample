using Microservices.Web.Frontend.Models.DTO;
using System;
using System.Collections.Generic;
using Microservices.Web.Frontend.Models.DTO.Order;

namespace Microservices.Web.Frontend.Services.OrderServices
{
    public interface IOrderService
    {
        List<OrderDTO> GetOrders(string UserId);
        OrderDetailDTO OrderDetail(Guid OrderId);
        ResultDTO RequestPayment(Guid OrderId);

    }
}
