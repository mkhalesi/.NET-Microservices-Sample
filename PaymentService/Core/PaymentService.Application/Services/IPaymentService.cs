using System;
using PaymentService.Domain.DTOs;

namespace PaymentService.Application.Services
{
    public interface IPaymentService
    {
        PaymentDTO GetPaymentOfOrder(Guid orderId);

        PaymentDTO GetPayment(Guid PaymentId);

        bool CreatePayment(Guid orderId, int Amount);

        void PayDone(Guid paymentId, string Authority, long RefId);
    }
}
