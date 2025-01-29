using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FoodDeliveryBackend.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all dishes with optional filters and sorting.
        /// </summary>
        public async Task<List<DishDto>> GetDishesAsync(List<string>? categories, bool? vegetarian, string? sorting, int page)
        {
            int pageSize = 10; // Pagination size
            var query = _context.Dishes.AsQueryable();

            // 🔹 Filter by category
            if (categories != null && categories.Any())
                query = query.Where(d => categories.Contains(d.Category.ToString()));

            // 🔹 Filter by vegetarian option
            if (vegetarian.HasValue)
                query = query.Where(d => d.IsVegetarian == vegetarian.Value);

            // 🔹 Sorting
            query = sorting switch
            {
                "NameAsc" => query.OrderBy(d => d.Name),
                "NameDesc" => query.OrderByDescending(d => d.Name),
                "PriceAsc" => query.OrderBy(d => d.Price),
                "PriceDesc" => query.OrderByDescending(d => d.Price),
                "RatingAsc" => query.OrderBy(d => d.AverageRating),
                "RatingDesc" => query.OrderByDescending(d => d.AverageRating),
                _ => query.OrderBy(d => d.Name) // Default sorting
            };

            // 🔹 Apply pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query
                .Select(d => new DishDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    IsVegetarian = d.IsVegetarian,
                    Category = d.Category.ToString(),
                    AverageRating = d.AverageRating
                })
                .ToListAsync();
        }

        /// <summary>
        /// Get a dish by ID.
        /// </summary>
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
                IsVegetarian = dish.IsVegetarian,
                Category = dish.Category,
                AverageRating = dish.AverageRating
            };
        }

        /// <summary>
        /// Create a new dish.
        /// </summary>
        public async Task CreateDishAsync(CreateDishDto model)
        {
            var dish = new Dish
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                IsVegetarian = model.IsVegetarian,
                Category = Enum.Parse<DishCategory>(model.Category),
                AverageRating = 0,
                RatingsCount = 0
            };

            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a dish.
        /// </summary>
        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return false;

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Update dish rating.
        /// </summary>
        public async Task<DishDto> RateDishAsync(int dishId, int rating)
        {
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null) return null;

            // Update rating calculations
            dish.RatingsCount += 1;
            dish.TotalRating += rating;
            dish.AverageRating = (double)dish.TotalRating / dish.RatingsCount;

            await _context.SaveChangesAsync();

            return new DishDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                IsVegetarian = dish.IsVegetarian,
                Category = dish.Category.ToString(),
                AverageRating = dish.AverageRating
            };
        }
    }
}
