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

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model">User registration details.</param>
        /// <returns>Response indicating success or error.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponse), 200)] // Success
        [ProducesResponseType(typeof(Response), 400)]     // Bad Request
        [ProducesResponseType(typeof(Response), 500)]     // Internal Server Error
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response { Status = "Error", Message = "Invalid request data." });

            try
            {
                await _authService.RegisterAsync(model);
                return Ok(new Response { Status = "Success", Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Login user and return JWT token.
        /// </summary>
        /// <param name="model">User login details.</param>
        /// <returns>JWT token on success.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), 200)] // Success
        [ProducesResponseType(typeof(Response), 400)]     // Bad Request
        [ProducesResponseType(typeof(Response), 500)]     // Internal Server Error
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response { Status = "Error", Message = "Invalid credentials." });

            try
            {
                var token = await _authService.LoginAsync(model);
                return Ok(new TokenResponse { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Logout the user.
        /// </summary>
        /// <returns>Response indicating success or error.</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)] // Success
        [ProducesResponseType(typeof(Response), 400)] // Bad Request
        [ProducesResponseType(typeof(Response), 401)] // Unauthorized
        [ProducesResponseType(typeof(Response), 403)] // Forbidden
        [ProducesResponseType(typeof(Response), 500)] // Internal Server Error
        public IActionResult Logout()
        {
            // Placeholder for logout logic (if any)
            return Ok(new Response { Status = "Success", Message = "User logged out successfully." });
        }

        /// <summary>
        /// Get user profile details.
        /// </summary>
        /// <returns>User profile information.</returns>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), 200)] // Success
        [ProducesResponseType(typeof(Response), 401)]       // Unauthorized
        [ProducesResponseType(typeof(Response), 403)]       // Forbidden
        [ProducesResponseType(typeof(Response), 500)]       // Internal Server Error
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
                var profile = await _authService.GetProfileAsync(userId);

                if (profile == null)
                    return NotFound(new Response { Status = "Error", Message = "User not found." });

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Update user profile details.
        /// </summary>
        /// <param name="model">Updated profile details.</param>
        /// <returns>Response indicating success or error.</returns>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(Response), 200)] // Success
        [ProducesResponseType(typeof(Response), 400)] // Bad Request
        [ProducesResponseType(typeof(Response), 401)] // Unauthorized
        [ProducesResponseType(typeof(Response), 403)] // Forbidden
        [ProducesResponseType(typeof(Response), 500)] // Internal Server Error
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response { Status = "Error", Message = "Invalid request data." });

            try
            {
                var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
                await _authService.UpdateProfileAsync(userId, model);
                return Ok(new Response { Status = "Success", Message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { Status = "Error", Message = ex.Message });
            }
        }
    }
}
