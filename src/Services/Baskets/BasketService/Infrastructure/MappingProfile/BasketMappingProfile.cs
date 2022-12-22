using AutoMapper;
using BasketService.Model.Entities;
using BasketService.Model.DTOs.Basket;
using BasketService.Model.DTOs.MessageDTO;
using BasketService.Model.DTOs.Product;

namespace BasketService.Infrastructure.MappingProfile
{
    public class BasketMappingProfile:Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<BasketItem, AddItemToBasketDto>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<AddItemToBasketDto, ProductDTO>().ReverseMap();

            CreateMap<BasketCheckoutMessage, CheckoutBasketDTO>().ReverseMap();
        }
    }
}
