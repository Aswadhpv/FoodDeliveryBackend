using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BLL.Services;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IOrderService _orderService; // Needed for rating validation

        public DishController(IDishService dishService, IOrderService orderService)
        {
            _dishService = dishService;
            _orderService = orderService;
        }

        /// <summary>
        /// Get all dishes (menu) with optional filters.
        /// </summary>
        /// <param name="categories">List of categories (optional).</param>
        /// <param name="vegetarian">Filter for vegetarian dishes.</param>
        /// <param name="sorting">Sorting option (NameAsc, PriceDesc, etc.).</param>
        /// <param name="page">Pagination page number.</param>
        /// <returns>Filtered and sorted list of dishes.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DishDto>), 200)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> GetAllDishes(
            [FromQuery] List<string>? categories,
            [FromQuery] bool? vegetarian,
            [FromQuery] string? sorting,
            [FromQuery] int page = 1)
        {
            try
            {
                var dishes = await _dishService.GetDishesAsync(categories, vegetarian, sorting, page);
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Get a dish by ID.
        /// </summary>
        /// <param name="id">Dish ID.</param>
        /// <returns>Dish details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDto), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> GetDishById(int id)
        {
            try
            {
                var dish = await _dishService.GetDishByIdAsync(id);
                if (dish == null)
                    return NotFound(new Response { Status = "Error", Message = "Dish not found." });

                return Ok(dish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Check if the user can rate a dish.
        /// </summary>
        /// <param name="id">Dish ID.</param>
        /// <returns>Boolean indicating if the user can rate.</returns>
        [HttpGet("{id}/rating/check")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 401)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> CanRateDish(int id)
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                    return Unauthorized(new Response { Status = "Error", Message = "Invalid user ID format." });

                bool canRate = await _orderService.HasUserOrderedDishAsync(userId, id);
                if (!canRate)
                    return BadRequest(new Response { Status = "Error", Message = "You must order this dish before rating it." });

                return Ok(new { CanRate = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Submit a rating for a dish.
        /// </summary>
        /// <param name="id">Dish ID.</param>
        /// <param name="rating">Rating value (1-5).</param>
        /// <returns>Updated dish rating.</returns>
        [HttpPost("{id}/rating")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 401)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> RateDish(int id, [FromBody] int rating)
        {
            try
            {
                if (rating < 1 || rating > 5)
                    return BadRequest(new Response { Status = "Error", Message = "Rating must be between 1 and 5." });

                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out int userId))
                    return Unauthorized(new Response { Status = "Error", Message = "Invalid user ID format." });

                bool canRate = await _orderService.HasUserOrderedDishAsync(userId, id);
                if (!canRate)
                    return BadRequest(new Response { Status = "Error", Message = "You must order this dish before rating it." });

                var updatedDish = await _dishService.RateDishAsync(id, rating);
                return Ok(updatedDish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
