using FoodDeliveryBackend.DTOs.OrderDtos;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersAsync(string userId);
        Task<OrderDto> GetOrderByIdAsync(string userId, int orderId);
        Task<OrderDto> CreateOrderAsync(string userId);
        Task<bool> ConfirmOrderDeliveryAsync(string userId, int orderId);
        Task<bool> HasUserOrderedDishAsync(string userId, int dishId);
    }
}
