using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddDish([FromBody] CreateDishDto model)
        {
            var dish = new Dish
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                IsVegetarian = model.IsVegetarian
            };

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return Ok("Dish added successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound("Dish not found.");
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return Ok("Dish deleted successfully.");
        }
    }
}
