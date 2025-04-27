using Rent_Project.Model;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Rent_Project.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly RentAppDbContext _context;

        public RefreshTokenRepository(RentAppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }




        public async Task<int?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var tokenInDb = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken && r.ExpiryDate > DateTime.UtcNow);

            return tokenInDb?.UserId;
        }






        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (existingToken != null)
            {
                _context.RefreshTokens.Remove(existingToken);
                await _context.SaveChangesAsync();
            }
        }


    }
}
