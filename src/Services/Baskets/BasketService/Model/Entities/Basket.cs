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

        public string Id { get; set; }
        public string UserId { get; private set; }
        public string DiscountId { get; set; }

        public List<BasketItem> Items { get; set; } = new();
    }
}
