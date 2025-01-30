using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Services
{
    public interface IDishService
    {
        Task<DishDto> GetDishByIdAsync(int id);
        Task CreateDishAsync(CreateDishDto model);
        Task<bool> DeleteDishAsync(int id);
        Task<List<DishDto>> GetDishesAsync(List<string>? categories, bool? vegetarian, string? sorting, int page);
        Task<DishDto> RateDishAsync(int dishId, int rating);
    }
}
