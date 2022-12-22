
using System;
using BasketService.Model.DTOs.Discount;

namespace BasketService.Model.Services.DiscountService
{
    public interface IDiscountService
    {
        DiscountDTO GetDiscountByCode(string code);
        DiscountDTO GetDiscountById(Guid discountId);
    }
}
