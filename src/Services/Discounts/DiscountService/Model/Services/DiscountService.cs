using System;
using System.Linq;
using AutoMapper;
using DiscountService.Infrastructure.Contexts;
using DiscountService.Model.Entities;

namespace DiscountService.Model.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DiscountDataBaseContext context;
        private readonly IMapper mapper;

        public DiscountService(DiscountDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool AddNewDiscount(string Code, int Amount)
        {
            DiscountCode discountCode = new DiscountCode()
            {
                Amount = Amount,
                Code = Code,
                Used = false,
            };
            context.DiscountCodes.Add(discountCode);
            context.SaveChanges();
            return true;
        }

        public DiscountDto GetDiscountByCode(string Code)
        {
            var discountCode = context.DiscountCodes.SingleOrDefault(p => p.Code.Equals(Code));

            if (discountCode == null)
                throw new Exception("Discount Not Found....");
            var result = mapper.Map<DiscountDto>(discountCode);
            return result;
        }

        public DiscountDto GetDiscountById(string Id)
        {
            var discountCode = context.DiscountCodes.SingleOrDefault(p => p.Id.Equals(Id));

            if (discountCode == null)
                throw new Exception("Discount Not Found....");
            var result = mapper.Map<DiscountDto>(discountCode);
            return result;
        }

        public bool UseDiscount(Guid Id)
        {
            var discountCode = context.DiscountCodes.Find(Id);
            if (discountCode == null)
                throw new Exception("Discouint Not Found....");
            discountCode.Used = true;
            context.SaveChanges();
            return true;
        }
    }
}