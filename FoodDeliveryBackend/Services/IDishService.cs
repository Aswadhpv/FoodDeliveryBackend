using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Services
{
    public interface IDishService
    {
        Task<List<DishDto>> GetDishesAsync();
        Task<DishDto> GetDishByIdAsync(int id);
        Task CreateDishAsync(CreateDishDto model);
        Task<bool> DeleteDishAsync(int id);
    }
}
