using System;
using System.Collections.Generic;
using OrderService.Model.DTOs.Common;
using OrderService.Model.DTOs.Order;

namespace OrderService.Model.Services.OrderService
{
    public interface IOrderService
    {
        //void AddOrder(AddOrderDto addOrder);
        List<OrderDto> GetOrdersForUser(string UserId);
        orderDetailDTO GetOrderById(Guid Id);
        ResultDTO RequestPayment(Guid orderId);
        bool PaymentIsDoneOrder(Guid orderId);
    }
}
