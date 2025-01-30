using FoodDeliveryBackend.DTOs.CartDtos;
using FoodDeliveryBackend.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;

namespace BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartDto>> GetCartAsync(string userId)
        {
            return await _context.CartItems
                .Where(c => c.UserId == userId)
                .Select(c => new CartDto
                {
                    DishId = c.DishId,
                    Name = c.Dish.Name,
                    Quantity = c.Quantity,
                    Price = c.Dish.Price,
                })
                .ToListAsync();
        }

        public async Task<bool> AddDishToCartAsync(string userId, int dishId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                _context.CartItems.Add(new CartItem { UserId = userId, DishId = dishId, Quantity = 1 });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDishFromCartAsync(string userId, int dishId, bool decrease)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId);

            if (cartItem == null) return false;

            if (decrease && cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            else
            {
                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
