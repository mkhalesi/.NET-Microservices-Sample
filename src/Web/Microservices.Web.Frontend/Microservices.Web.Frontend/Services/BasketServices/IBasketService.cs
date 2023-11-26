using System;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public interface IBasketService
    { 
        Task<BasketDTO>  GetBasket(string userId);
        Task<ResultDTO> AddToBasket(AddToBasketDTO basket, string UserId);
        Task<ResultDTO> DeleteFromBasket(Guid Id);
        Task<ResultDTO> UpdateQuantity(Guid basketItemId, int quantity);
        Task<ResultDTO> ApplyDiscountToBasket(Guid basketId, Guid discountId);
        Task<ResultDTO> Checkout(CheckoutDTO checkout);
    }
}
