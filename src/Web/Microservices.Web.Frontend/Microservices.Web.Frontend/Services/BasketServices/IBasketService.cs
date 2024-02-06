using System;
using System.Threading.Tasks;
using Microservices.Web.Frontend.Models.DTO;
using Microservices.Web.Frontend.Models.DTO.Basket;

namespace Microservices.Web.Frontend.Services.BasketServices
{
    public interface IBasketService
    { 
        Task<BasketDTO>  GetBasket(string userId);
        Task<ResultDTO> AddToBasket(AddToBasketDTO basket, string userId);
        Task<ResultDTO> DeleteFromBasket(Guid Id, string userId);
        Task<ResultDTO> UpdateQuantity(Guid basketItemId, int quantity, string userId);
        Task<ResultDTO> ApplyDiscountToBasket(Guid basketId, Guid discountId, string userId);
        Task<ResultDTO> Checkout(CheckoutDTO checkout);
    }
}
