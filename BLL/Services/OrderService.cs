using FoodDeliveryBackend.DTOs.OrderDtos;
using FoodDeliveryBackend.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;


namespace BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.UserId == userId && o.Id == orderId)
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<OrderDto> CreateOrderAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Dish)
                .ToListAsync();

            if (!cartItems.Any()) throw new Exception("Cart is empty.");

            var order = new Order
            {
                UserId = userId,
                TotalPrice = cartItems.Sum(c => c.Dish.Price * c.Quantity),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear user's cart after placing order
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return new OrderDto
            {
                OrderId = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<bool> CancelOrderAsync(string userId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);

            if (order == null) return false;

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasUserOrderedDishAsync(string userId, int dishId)
        {
            // Check if there is an order by the user that contains the specified dish
            var hasOrderedDish = await _context.Orders
                .Where(o => o.UserId == userId)  // Filter by userId
                .AnyAsync(o => o.OrderDetails.Any(oi => oi.DishId == dishId));  // Check if the order contains the dish

            return hasOrderedDish;
        }

        public async Task<bool> ConfirmOrderDeliveryAsync(string userId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);

            if (order == null) return false;

            order.Status = "Delivered";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
