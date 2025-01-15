using FoodDeliveryBackend.DTOs;

namespace BLL.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task UpdateUserProfileAsync(int userId, UpdateUserProfileDto model);
        Task ChangePasswordAsync(int userId, ChangePasswordDto model);
    }
}
