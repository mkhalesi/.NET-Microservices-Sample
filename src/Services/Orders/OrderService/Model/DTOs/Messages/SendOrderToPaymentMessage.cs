using System;
using OrderService.MessageBus.Base;

namespace OrderService.Model.DTOs.Messages
{
    public class SendOrderToPaymentMessage : BaseMessage
    {
        public string OrderId { get; set; }
        public int Amount { get; set; }
    }
}
