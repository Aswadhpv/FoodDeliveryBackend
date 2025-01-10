using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Services;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            await _authService.RegisterAsync(model);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var token = await _authService.LoginAsync(model);
            return Ok(new { Token = token });
        }
    }
}
