using DAL.Data;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRating(RatingDto model)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var rating = new Rating
            {
                DishId = model.DishId,
                UserId = userId,
                Stars = model.Stars,
                Review = model.Review
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok("Rating added successfully.");
        }

        [HttpGet("{dishId}")]
        public async Task<IActionResult> GetDishRatings(int dishId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.DishId == dishId)
                .ToListAsync();

            return Ok(ratings);
        }
    }

}
