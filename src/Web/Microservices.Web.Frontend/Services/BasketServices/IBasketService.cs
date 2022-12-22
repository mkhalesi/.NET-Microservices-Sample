using System;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public interface IBasketService
    {
        BasketDTO GetBasket(string userId);
        ResultDTO AddToBasket(AddToBasketDTO basket, string UserId);
        ResultDTO DeleteFromBasket(Guid Id);
        ResultDTO UpdateQuantity(Guid basketItemId, int quantity);
        ResultDTO ApplyDiscountToBasket(Guid basketId, Guid discountId);
        ResultDTO Checkout(CheckoutDTO checkout);
    }
}
