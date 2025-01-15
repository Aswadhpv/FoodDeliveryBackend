using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Data;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Dish)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost("add-dish")]
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

        [HttpDelete("delete-dish/{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound("Dish not found.");

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return Ok("Dish deleted successfully.");
        }
    }
}
