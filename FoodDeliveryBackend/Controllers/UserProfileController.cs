using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FoodDeliveryBackend.Models;
using FoodDeliveryBackend.DTOs;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // Get user profile
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound("User not found.");

            var profile = new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email
            };

            return Ok(profile);
        }

        // Update user profile
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UpdateUserProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound("User not found.");

            user.Name = model.Name;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update profile.");
            }

            return Ok("Profile updated successfully.");
        }

        // Change user password
        [HttpPut("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest("Password change failed.");
            }

            return Ok("Password updated successfully.");
        }
    }
}
