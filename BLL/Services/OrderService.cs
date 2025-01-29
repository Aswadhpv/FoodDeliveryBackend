using DAL.Data;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task CreateOrderAsync(CreateOrderDto model);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> HasUserOrderedDishAsync(int userId, int dishId);
    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Dish)
                .ToListAsync();
        }

        public async Task CreateOrderAsync(CreateOrderDto model)
        {
            var order = new Order
            {
                UserId = model.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "In Progress",
                OrderDetails = model.OrderDetails.Select(od => new OrderDetail
                {
                    DishId = od.DishId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new ArgumentException("Order not found.");
            order.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserOrderedDishAsync(int userId, int dishId)
        {
            return await _context.Orders
                .AnyAsync(o => o.UserId == userId && o.Dishes.Any(d => d.Id == dishId));
        }

    }
}
