
using OrderService.Model.DTOs.Basket;

namespace OrderService.Model.Services.RegisterOrderService
{
    public interface IRegisterOrderService
    {
        bool Execute(BasketDTO basket);
    }
}