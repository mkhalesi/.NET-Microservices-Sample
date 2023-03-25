using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using System;
using Microservices.Web.Frontend.Models.DTO.Order;
using System.Threading.Tasks;

namespace Microservices.Web.Frontend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IPaymentService paymentService;
        private readonly string UserId = "1";

        public OrdersController(IOrderService orderService,
            IPaymentService paymentService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await orderService.GetOrders(UserId);
            return View(orders);
        }

        public async Task<IActionResult> Detail(Guid Id)
        {
            var order = await orderService.OrderDetail(Id);
            return View(order);
        }

        public async Task<IActionResult> Pay(Guid OrderId)
        {
            var order = await orderService.OrderDetail(OrderId);
            if (order.PaymentStatus == PaymentStatus.isPaid)
            {
                return RedirectToAction(nameof(Detail), new { Id = OrderId });
            }
            if (order.PaymentStatus == PaymentStatus.unPaid)
            {
                //Submit a payment request for the order service
                var paymentRequest = await orderService.RequestPayment(OrderId);
            }

            //Get the payment link from the payment service
            string callbackUrl = Url.Action(nameof(Detail), "Orders",
                new { Id = OrderId }, protocol: Request.Scheme);
            var paymentlink = paymentService.GetPaymentlink(order.Id, callbackUrl);

            if (paymentlink.IsSuccess)
            {
                return Redirect(paymentlink.Data.PaymentLink);
            }

            return NotFound();
        }
    }
}
