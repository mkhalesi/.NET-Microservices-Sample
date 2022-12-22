using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.Web.Frontend.Models.DTO.Basket
{
    public class BasketDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Guid? DiscountId { get; set; }
        public DiscountInBasketDTO DiscountDetail { get; set; } = null;

        public List<BasketItemDTO> Items { get; set; }
        public int TotalPrice()
        {
            int result = Items.Sum(p => p.UnitPrice * p.Quantity);
            if (DiscountId.HasValue)
                result = result - DiscountDetail.Amount;
            return result;
        }
    }
}
