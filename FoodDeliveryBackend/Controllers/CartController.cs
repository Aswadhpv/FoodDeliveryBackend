using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Data;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all items in the user's cart
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Dish)
                .ToListAsync();

            return Ok(cartItems);
        }

        // Add an item to the cart
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddCartItemDto model)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == model.UserId && ci.DishId == model.DishId);

            if (existingItem != null)
            {
                existingItem.Quantity += model.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = model.UserId,
                    DishId = model.DishId,
                    Quantity = model.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Item added to cart.");
        }

        // Update the quantity of an item in the cart
        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDto model)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return NotFound("Cart item not found.");

            cartItem.Quantity = model.Quantity;
            await _context.SaveChangesAsync();
            return Ok("Cart item updated.");
        }

        // Remove an item from the cart
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return NotFound("Cart item not found.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok("Item removed from cart.");
        }
    }
}
