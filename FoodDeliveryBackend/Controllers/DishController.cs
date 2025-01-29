using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Get all dishes (menu).
        /// </summary>
        /// <returns>List of dishes.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DishDto>), 200)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> GetAllDishes()
        {
            try
            {
                var dishes = await _dishService.GetDishesAsync();
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
    }
}
