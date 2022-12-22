namespace Microservices.Web.Frontend.Models.DTO.Basket
{
    public class BasketItemDTO
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public int TotalPrice()
        {
            return UnitPrice * Quantity;
        }
    }
}
