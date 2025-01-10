namespace DAL.Interfaces
{
    public interface ITokenService
    {
        Task<bool> IsUserLoggedOut(Guid userId);
    }
}
