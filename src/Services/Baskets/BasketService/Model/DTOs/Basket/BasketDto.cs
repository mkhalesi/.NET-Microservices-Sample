using System;
using System.Collections.Generic;
using System.Linq;

namespace BasketService.Model.DTOs.Basket
{
    public class BasketDto
    {
        public BasketDto()
        {
            Items = new List<BasketItemDto>();
        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public int Total()
        {
            if (Items.Count > 0)
            {
                int total = Items.Sum(p => p.UnitPrice * p.Quantity);
                return total;
            }
            return 0;
        }

    }
}