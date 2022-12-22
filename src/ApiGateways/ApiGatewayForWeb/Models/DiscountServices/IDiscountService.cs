using ApiGateway.ForWeb.Models.Dtos;

namespace ApiGateway.ForWeb.Models.DiscountServices
{
    public interface IDiscountService
    {
        ResultDto<DiscountDto> GetDiscountByCode(string Code);
        ResultDto<DiscountDto> GetDiscountById(Guid Id);
        ResultDto UseDiscount(Guid DiscountId);
    }
}
