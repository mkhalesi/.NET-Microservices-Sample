using OrderService.Infrastructure.Context;
using OrderService.Model.DTOs.Order;
using OrderService.Model.DTOs.OrderLine;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.MessageBus.Base;
using OrderService.MessageBus.SendMessages;
using OrderService.Model.DTOs.Common;
using OrderService.Model.DTOs.Messages;
using OrderService.Model.Entities;

namespace OrderService.Model.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDataBaseContext context;
        private readonly IMessageBus messageBus;
        private readonly string QueueName_OrderSendToPayment;
        public OrderService(IOrderDataBaseContext context, IMessageBus messageBus,
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

        public orderDetailDTO GetOrderById(string Id)
        {
            var filter = Builders<Order>.Filter.Eq(p => p.Id, Id);
            var order = context.Orders.Find(filter).FirstOrDefault();

            if (order == null)
                throw new Exception("Order Not Found");

            var result = new orderDetailDTO
            {
                Id = order.Id,
                OrderPaid = order.OrderPaid,
                OrderPlaced = order.OrderPlaced,
                UserId = order.UserId,
                FirstName = order.FirstName,
                LastName = order.LastName,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                TotalPrice = order.TotalPrice,
                PaymentStatus = order.PaymentStatus,
                OrderLines = order.OrderLines.Select(o => new OrderLineDto
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
            var filter = Builders<Order>.Filter.Eq(p => p.UserId, UserId);
            var orders = context.Orders
             .Find(filter)
             .ToList()
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

        public ResultDTO RequestPayment(string orderId)
        {
            var filter = Builders<Order>.Filter.Eq(p => p.Id, orderId);
            var order = context.Orders.Find(filter).FirstOrDefault();
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
            context.Orders.ReplaceOne(filter, order);

            return new ResultDTO()
            {
                IsSuccess = true,
                Message = "order pay successfully"
            };
        }

        public bool PaymentIsDoneOrder(string orderId)
        {
            var filter = Builders<Order>.Filter.Eq(p => p.Id, orderId);
            var order = context.Orders.Find(filter)
                .FirstOrDefault();
            order.PaymentIsDone();

            var orderResult = context.Orders.ReplaceOne(filter, order);

            return orderResult.IsAcknowledged && orderResult.ModifiedCount > 0;
        }
    }
}
