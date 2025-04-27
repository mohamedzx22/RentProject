namespace Rent_Project.Services
{
    public interface IRefreshTokenService
    {
        Task<object> RefreshTokenAsync(string refreshToken);
    }
}
