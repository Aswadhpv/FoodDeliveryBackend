using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BLL.Services;
using BLL.Interfaces;
using FoodDeliveryBackend.DTOs.CartDtos;


namespace FoodDeliveryBackend.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get user cart.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<CartDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var cartItems = await _cartService.GetCartAsync(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Add dish to cart.
        /// </summary>
        [HttpPost("dish/{dishId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> AddDishToCart(int dishId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var result = await _cartService.AddDishToCartAsync(userId, dishId);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Remove dish from cart or decrease quantity.
        /// </summary>
        [HttpDelete("dish/{dishId}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> RemoveDishFromCart(int dishId, [FromQuery] bool increase = false)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var result = await _cartService.RemoveDishFromCartAsync(userId, dishId, increase);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
