using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateDish([FromBody] CreateDishDto model)
        {
            // Create dish logic
            return Ok();
        }
    }
}
