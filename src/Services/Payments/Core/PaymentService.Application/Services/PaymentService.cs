using PaymentService.Application.Context;
using PaymentService.Domain.DTOs;
using PaymentService.Domain.Entities;
using System;
using System.Linq;

namespace PaymentService.Application.Services
{
    public class PaymentServiceConcrete : IPaymentService
    {
        private readonly IPaymentDatabaseContext context;
        public PaymentServiceConcrete(IPaymentDatabaseContext context)
        {
            this.context = context;
        }

        public bool CreatePayment(Guid orderId, int Amount)
        {
            var order = GetOrder(orderId, Amount);
            var payment = context.Payments.FirstOrDefault(x => x.OrderId == order.Id);
            if (payment != null)
                return true;

            Payment newPayment = new Payment()
            {
                Amount = Amount,
                IsPay = false,
                OrderId = orderId
            };
            context.Payments.Add(newPayment);
            context.SaveChanges();
            return true;
        }

        public PaymentDTO GetPayment(Guid PaymentId)
        {
            var payment = context.Payments.FirstOrDefault(p => p.Id == PaymentId);
            if (payment == null)
                return null;

            return new PaymentDTO
            {
                Amount = payment.Amount,
                IsPay = payment.IsPay,
                OrderId = payment.OrderId,
                PaymentId = payment.Id,
            };
        }

        public PaymentDTO GetPaymentOfOrder(Guid orderId)
        {
            var payment = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
            if (payment == null)
                return null;

            return new PaymentDTO
            {
                Amount = payment.Amount,
                IsPay = payment.IsPay,
                OrderId = payment.OrderId,
                PaymentId = payment.Id,
            };
        }

        public void PayDone(Guid paymentId, string Authority, long RefId)
        {
            var payment = context.Payments.FirstOrDefault(p => p.OrderId == paymentId);
            payment.DatePay = DateTime.Now;
            payment.IsPay = true;
            payment.Authority = Authority;
            payment.RefId = RefId;
            context.SaveChanges();
        }

        private Order GetOrder(Guid orderId, int Amount)
        {
            var order = context.Order.FirstOrDefault(p => p.Id == orderId);
            if (order != null)
            {
                if (order.Amount != Amount)
                {
                    order.Amount = Amount;
                    context.SaveChanges();
                }
                return order;
            }
            else
            {
                Order newOrder = new Order
                {
                    Amount = Amount,
                    Id = orderId
                };
                context.Order.Add(newOrder);
                context.SaveChanges();
                return newOrder;
            }
        }
    }
}
