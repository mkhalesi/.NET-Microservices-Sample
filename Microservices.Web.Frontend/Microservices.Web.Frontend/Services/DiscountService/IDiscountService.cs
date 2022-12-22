using System;
using Microservices.Web.Frontend.Models.DTO;

namespace Microservices.Web.Frontend.Services.DiscountService
{
    public interface IDiscountService
    {
        ResultDTO<DiscountDTO> GetDiscountByCode(string code);
        ResultDTO<DiscountDTO> GetDiscountById(Guid Id);
        ResultDTO UseDiscount(Guid DiscountId);
    }
}
