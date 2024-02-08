using System;

namespace BasketService.Model.Entities
{
    [Serializable]
    public class BasketItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public string BasketId { get; set; }
        public string ProductId { get; set; }

        public Basket Basket { get; set; }
        public Product Product { get; set; }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
