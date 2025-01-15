using FoodDeliveryBackend.DTOs;
using DAL.Data;
using FoodDeliveryBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryBackend.Services
{
    public interface IDishService
    {
        Task CreateDishAsync(CreateDishDto model);
        Task<List<Dish>> GetDishesAsync();
    }

    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateDishAsync(CreateDishDto model)
        {
            var dish = new Dish
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price, // Direct assignment since it's non-nullable
                IsVegetarian = model.IsVegetarian // Direct assignment since it's non-nullable
            };

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Dish>> GetDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }
    }
}
