using AutoMapper;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // User Mappings
            CreateMap<User, RegisterDto>().ReverseMap();

            // Dish Mappings
            CreateMap<Dish, CreateDishDto>().ReverseMap();
        }
    }
}
