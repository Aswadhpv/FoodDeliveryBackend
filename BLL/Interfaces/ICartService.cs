using FoodDeliveryBackend.DTOs.CartDtos;

namespace BLL.Interfaces
{
    public interface ICartService
    {
        Task<List<CartDto>> GetCartAsync(string userId);
        Task<bool> AddDishToCartAsync(string userId, int dishId);
        Task<bool> RemoveDishFromCartAsync(string userId, int dishId, bool decrease);
    }
}
