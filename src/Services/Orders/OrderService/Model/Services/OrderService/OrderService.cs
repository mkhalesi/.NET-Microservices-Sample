using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Context;
using OrderService.Model.DTOs.Order;
using OrderService.Model.DTOs.OrderLine;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using OrderService.MessageBus.Base;
using OrderService.MessageBus.SendMessages;
using OrderService.Model.DTOs.Common;
using OrderService.Model.DTOs.Messages;

namespace OrderService.Model.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly OrderDataBaseContext context;
        private readonly IMessageBus messageBus;
        private readonly string QueueName_OrderSendToPayment;
        public OrderService(OrderDataBaseContext context, IMessageBus messageBus,
            IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            this.context = context;
            this.messageBus = messageBus;
            QueueName_OrderSendToPayment = rabbitMqOptions.Value.QueueName_OrderSendToPayment;
        }

        // public void AddOrder(AddOrderDto addOrder)
        // {
        //     List<OrderLine> orderLines = new List<OrderLine>();
        //     foreach (var item in addOrder.OrderLines)
        //     {
        //         orderLines.Add(new OrderLine
        //         {
        //             ProductId = item.ProductId,
        //             ProductName = item.ProductName,
        //             ProductPrice = item.ProductPrice,
        //             Quantity = item.Quantity,
        //         });
        //         Order order = new Order(addOrder.UserId, orderLines);
        //         context.Orders.Add(order);
        //         context.SaveChanges();

        //     }
        // }

        public orderDetailDTO GetOrderById(Guid Id)
        {
            var orders = context.Orders
            .Include(p => p.OrderLines)
            .ThenInclude(p => p.Product)
            .FirstOrDefault(p => p.Id == Id);

            if (orders == null)
                throw new Exception("Order Not Found");

            var result = new orderDetailDTO
            {
                Id = orders.Id,
                OrderPaid = orders.OrderPaid,
                OrderPlaced = orders.OrderPlaced,
                UserId = orders.UserId,
                FirstName = orders.FirstName,
                LastName = orders.LastName,
                PhoneNumber = orders.PhoneNumber,
                Address = orders.Address,
                TotalPrice = orders.TotalPrice,
                PaymentStatus = orders.PaymentStatus,
                OrderLines = orders.OrderLines.Select(o => new OrderLineDto
                {
                    Id = o.Id,
                    ProductId = o.Product.ProductId,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Quantity = o.Quantity,
                }).ToList(),
            };
            return result;
        }

        public List<OrderDto> GetOrdersForUser(string UserId)
        {
            var orders = context.Orders
             .Include(p => p.OrderLines)
             .Where(p => p.UserId == UserId)
             .Select(p => new OrderDto
             {
                 Id = p.Id,
                 OrderPaid = p.OrderPaid,
                 OrderPlaced = p.OrderPlaced,
                 ItemCount = p.OrderLines.Count(),
                 TotalPrice = p.TotalPrice,
                 PaymentStatus = p.PaymentStatus
             }).ToList();
            return orders;
        }

        public ResultDTO RequestPayment(Guid orderId)
        {
            var order = context.Orders.SingleOrDefault(p => p.Id == orderId);
            if (order == null)
                return new ResultDTO()
                {
                    ErrorMessage = "order not found",
                    IsSuccess = false,
                };

            // send payment message to PaymentService
            SendOrderToPaymentMessage message = new SendOrderToPaymentMessage()
            {
                Amount = order.TotalPrice,
                OrderId = order.Id
            };
            messageBus.SendMessage(message, QueueName_OrderSendToPayment);

            // change order paymentStatus
            order.PaymentIsDone();
            context.SaveChanges();

            return new ResultDTO()
            {
                IsSuccess = true,
                Message = "order pay successfully"
            };
        }

        public bool PaymentIsDoneOrder(Guid orderId)
        {
            var order = context.Orders.FirstOrDefault(p => p.Id == orderId);
            if (order == null)
                return false;
            order.PaymentIsDone();
            context.SaveChanges();
            return true;
        }
    }
}
