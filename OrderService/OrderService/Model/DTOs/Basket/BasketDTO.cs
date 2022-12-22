
using System;
using System.Collections.Generic;
using OrderService.MessageBus.Base;

namespace OrderService.Model.DTOs.Basket
{
    public class BasketDTO : BaseMessage
    {
        public Guid BasketId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int TotalPrice { get; set; }
        public List<BasketItemDTO> BasketItems { get; set; }
    }
}