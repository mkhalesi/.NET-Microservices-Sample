using Microservices.Web.Frontend.Models.DTO;
using System;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<ResultDTO<ReturnPaymentLinkDTO>> GetPaymentLink(Guid OrderId, string CallbackUrl);
    }

    public class ReturnPaymentLinkDTO
    {
        public string PaymentLink { get; set; }
    }
}
