using DAL.Data;
using FoodDeliveryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, int dishId, int quantity);
        Task UpdateCartItemAsync(int cartItemId, int quantity);
        Task RemoveFromCartAsync(int cartItemId);
    }

    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartAsync(int userId)
        {
            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Dish)
                .ToListAsync();
        }

        public async Task AddToCartAsync(int userId, int dishId, int quantity)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    DishId = dishId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) throw new ArgumentException("Cart item not found.");

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) throw new ArgumentException("Cart item not found.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
