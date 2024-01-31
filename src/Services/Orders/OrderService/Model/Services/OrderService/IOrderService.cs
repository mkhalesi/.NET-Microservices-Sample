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
        orderDetailDTO GetOrderById(string Id);
        ResultDTO RequestPayment(string orderId);
        bool PaymentIsDoneOrder(string orderId);
    }
}
