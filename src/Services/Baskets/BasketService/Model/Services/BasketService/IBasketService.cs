using System;
using BasketService.Model.DTOs;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.Services.DiscountService;

namespace BasketService.Model.Services.BasketService
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string UserId);
        BasketDto GetBasket(string UserId);
        void AddItemToBasket(AddItemToBasketDto item);
        void RemoveItemFromBasket(Guid ItemId);
        void SetQuantities(Guid itemId, int quantity);
        void TransferBasket(string anonymousId, string UserId);

        ResultDTO CheckoutBasket(CheckoutBasketDTO checkoutBasket, IDiscountService discountService);
    }
}
