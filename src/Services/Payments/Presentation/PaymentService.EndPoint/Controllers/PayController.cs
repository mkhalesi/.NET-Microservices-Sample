using System;
using System.Threading.Tasks;
using Dto.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentService.Application.Services;
using PaymentService.Domain.DTOs;
using PaymentService.Domain.DTOs.Common;
using PaymentService.Infrastructure.MessagingBus.Messages;
using PaymentService.Infrastructure.MessagingBus.SendMessages;
using RestSharp;
using ZarinPal.Class;

namespace PaymentService.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        private readonly ZarinPal.Class.Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration configuration;
        private readonly IMessageBus messageBus;
        private readonly string merchendId;
        private readonly string _queueName;

        public PayController(IPaymentService paymentService
            , IConfiguration configuration, IMessageBus messageBus)
        {
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
            this.paymentService = paymentService;
            this.configuration = configuration;
            this.messageBus = messageBus;
            merchendId = configuration["Zarinpal:merchendId"];
            _queueName = configuration["RabbitMq:QueueName_PaymentDone"];
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(Guid OrderId, string callbackUrlFront)
        {
            var pay = paymentService.GetPaymentOfOrder(OrderId);
            if (pay == null)
            {
                ResultDTO ResultDTO = new ResultDTO
                {
                    IsSuccess = false,
                    Message = "Payment Not Found"
                };
                return Ok(ResultDTO);
            }
            string callbackUrl = Url.Action(nameof(Verify), "pay",
                new
                {
                    paymentId = pay.PaymentId,
                    callbackUrlFront = callbackUrlFront
                },
                protocol: Request.Scheme);

            try
            {
                var result = await _payment.Request(new DtoRequest()
                {
                    Amount = pay.Amount,
                    CallbackUrl = callbackUrl,
                    Description = "test",
                    Email = "",
                    Mobile = "",
                    MerchantId = merchendId,
                }, Payment.Mode.sandbox);

                string redirectUrl = $"https://zarinpal.com/pg/StartPay/{result.Authority}";

                return Ok(new ResultDTO<ReturnPaymentLinkDTO>
                {
                    IsSuccess = true,
                    Data = new ReturnPaymentLinkDTO { PaymentLink = redirectUrl },
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ResultDTO<ReturnPaymentLinkDTO>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("Verify")]
        public IActionResult Verify(Guid paymentId, string callbackUrlFront)
        {

            string Status = HttpContext.Request.Query["Status"];
            string Authority = HttpContext.Request.Query["authority"];

            if (Status != "" &&
                  Status.ToString().ToLower() == "ok" &&
                 Authority != "")
            {
                var pay = paymentService.GetPayment(paymentId);
                if (pay == null)
                {
                    return NotFound();
                }

                var client = new RestClient("https://www.zarinpal.com/pg/rest/WebGate/PaymentVerification.json");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", $"{{\"MerchantID\" :\"{merchendId}\",\"Authority\":\"{Authority}\",\"Amount\":\"{pay.Amount}\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                VerificationPayResultDTO verification = JsonConvert.DeserializeObject<VerificationPayResultDTO>(response.Content);

                if (verification.Status == 100)
                {
                    paymentService.PayDone(paymentId, Authority, verification.RefID);

                    var payment = new PaymentIsDoneMessage()
                    {
                        OrderId = pay.OrderId
                    };
                    messageBus.SendMessage(payment, _queueName);
                    return Redirect(callbackUrlFront);
                }
                else
                {
                    return NotFound(callbackUrlFront);
                }
            }

            return Redirect(callbackUrlFront);
        }
    }
}
