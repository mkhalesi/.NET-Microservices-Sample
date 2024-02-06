using BasketService.Model.DTOs;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.Services.DiscountService;

namespace BasketService.Model.Services.BasketService
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string userId);
        BasketDto GetBasket(string userId);
        void AddItemToBasket(AddItemToBasketDto item);
        void RemoveItemFromBasket(string itemId, string userId);
        void SetQuantities(string itemId, int quantity, string userId);
        void TransferBasket(string anonymousId, string userId);
        void ApplyDiscountToBasket(string basketId, string discountId, string userId);
        ResultDTO CheckoutBasket(CheckoutBasketDTO checkoutBasket, IDiscountService discountService);
    }
}
