using DAL.Interfaces;

namespace DAL.Services
{
    public class TokenService : ITokenService
    {
        public Task<bool> IsUserLoggedOut(Guid userId)
        {
            // Placeholder logic to check if the user is logged out
            return Task.FromResult(false);
        }
    }
}
