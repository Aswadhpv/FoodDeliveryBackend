using FoodDeliveryBackend.DTOs;

namespace DAL.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto model);
        Task<string> LoginAsync(LoginDto model);
        Task<UserProfileDto> GetProfileAsync(int userId);
        Task UpdateProfileAsync(int userId, UpdateUserProfileDto model);
    }
}
