using System;

namespace Microservices.Web.Frontend.Models.DTO.Discount
{
    public class DiscountDTO
    {
        public int Amount { get; set; }
        public string Code { get; set; }
        public Guid Id { get; set; }
        public bool Used { get; set; }
    }
}
