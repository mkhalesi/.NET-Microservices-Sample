using AutoMapper;
using DiscountService.Model.Entities;
using DiscountService.Model.Services;

namespace DiscountService.Infrastructure.MappingProfile
{
    public class DiscountMappingProfile:Profile
    {
        public DiscountMappingProfile()
        {
            CreateMap<DiscountCode, DiscountDto>().ReverseMap();

        }
    }
}
