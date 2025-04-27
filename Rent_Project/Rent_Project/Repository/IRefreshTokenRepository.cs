using Rent_Project.Model;

namespace Rent_Project.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GenerateRefreshTokenAsync(int userId);
        Task<int?> ValidateRefreshTokenAsync(string refreshToken);
        Task DeleteRefreshTokenAsync(string refreshToken);
    }
}
