using Microservices.Web.Frontend.Services.OrderServices;
using Microservices.Web.Frontend.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using System;
using Microservices.Web.Frontend.Models.DTO.Order;

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
        public IActionResult Index()
        {
            var orders = orderService.GetOrders(UserId);
            return View(orders);
        }

        public IActionResult Detail(Guid Id)
        {
            var order = orderService.OrderDetail(Id);
            return View(order);
        }

        public IActionResult Pay(Guid OrderId)
        {
            var order = orderService.OrderDetail(OrderId);
            if (order.PaymentStatus == PaymentStatus.isPaid)
            {
                return RedirectToAction(nameof(Detail), new { Id = OrderId });
            }
            if (order.PaymentStatus == PaymentStatus.unPaid)
            {
                //ارسال درخواست پرداخت برای سرویس سفارش
                var paymentRequest = orderService.RequestPayment(OrderId);
            }

            // دریافت لینک پرداخت از سرویس پرداخت
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
