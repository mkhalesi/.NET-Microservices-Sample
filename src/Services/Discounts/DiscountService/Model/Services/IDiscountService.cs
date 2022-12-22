using System;

namespace DiscountService.Model.Services
{
    public interface IDiscountService
    {
        DiscountDto GetDiscountByCode(string Code);
        DiscountDto GetDiscountById(string Id);
        bool UseDiscount(Guid Id);
        bool AddNewDiscount(string Code, int Amount);
    }

    public class DiscountDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public bool Used { get; set; }
    }
}
