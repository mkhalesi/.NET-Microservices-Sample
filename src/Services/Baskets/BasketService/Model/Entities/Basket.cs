using System;
using System.Collections.Generic;

namespace BasketService.Model.Entities
{
    [Serializable]
    public class Basket
    {
        public Basket(string UserId)
        {
            this.UserId = UserId;
        }
        public Basket()
        {
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string DiscountId { get; set; }

        public List<BasketItem> Items { get; set; } = new();
    }
}
