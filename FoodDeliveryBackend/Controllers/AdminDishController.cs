using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/admin/dish")]
    [ApiController]
    [Authorize(Roles = "DishManager")] // Only users with 'DishManager' role can access
    public class AdminDishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public AdminDishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Add a new dish.
        /// </summary>
        /// <param name="model">Dish details.</param>
        /// <returns>Success message.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> AddDish([FromBody] CreateDishDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response { Status = "Error", Message = "Invalid dish data." });

            try
            {
                await _dishService.CreateDishAsync(model);
                return Ok(new Response { Status = "Success", Message = "Dish added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a dish by ID.
        /// </summary>
        /// <param name="id">Dish ID.</param>
        /// <returns>Success message.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 404)]
        [ProducesResponseType(typeof(Response), 500)]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                var result = await _dishService.DeleteDishAsync(id);
                if (!result)
                    return NotFound(new Response { Status = "Error", Message = "Dish not found." });

                return Ok(new Response { Status = "Success", Message = "Dish deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
