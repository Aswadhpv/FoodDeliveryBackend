using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryBackend.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DishDto>> GetDishesAsync()
        {
            return await _context.Dishes
                .Select(d => new DishDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    IsVegetarian = d.IsVegetarian
                })
                .ToListAsync();
        }

        public async Task<DishDto> GetDishByIdAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return null;

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                IsVegetarian = dish.IsVegetarian
            };
        }

        public async Task CreateDishAsync(CreateDishDto model)
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
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return false;

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
