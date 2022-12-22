using Microservices.Web.Frontend.Models.DTO;
using System;

namespace Microservices.Web.Frontend.Services.PaymentServices
{
    public interface IPaymentService
    {
        ResultDTO<ReturnPaymentLinkDTO> GetPaymentlink(Guid OrderId, string CallbackUrl);
    }

    public class ReturnPaymentLinkDTO
    {
        public string PaymentLink { get; set; }
    }
}
