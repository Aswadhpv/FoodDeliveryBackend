using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;
using Microsoft.AspNetCore.Authorization;
using DAL.Interfaces;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RegisterAsync(model);
            return Ok("User registered successfully.");
        }

        // Login user and return JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(model);
            return Ok(new { Token = token });
        }

        // Get the user's profile
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var profile = await _authService.GetProfileAsync(userId);

            if (profile == null)
                return NotFound("User not found.");

            return Ok(profile);
        }

        // Update the user's profile
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            await _authService.UpdateProfileAsync(userId, model);
            return Ok("Profile updated successfully.");
        }

        // Logout user (optional implementation)
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // JWT tokens are stateless, so implement logout logic if needed
            return Ok("User logged out successfully.");
        }
    }
}
