using FoodDeliveryBackend.DTOs;

namespace DAL.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto model);
        Task<string> LoginAsync(LoginDto model);
    }
}
