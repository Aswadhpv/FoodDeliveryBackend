using DAL.Interfaces;
using FoodDeliveryBackend.DTOs;
using FoodDeliveryBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            var user = new User
            {
                Name = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                Address = model.Address,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                throw new Exception("Invalid credentials.");

            return "JWT_TOKEN_PLACEHOLDER"; // Replace with JWT generation logic if needed
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task UpdateProfileAsync(int userId, UpdateUserProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found.");

            user.Name = model.Name;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to update profile.");
        }
    }
}
