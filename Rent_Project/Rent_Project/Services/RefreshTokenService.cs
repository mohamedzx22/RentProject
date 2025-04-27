using Microsoft.IdentityModel.Tokens;
using Rent_Project.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rent_Project.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IConfiguration config)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _config = config;
        }




        public async Task<object> RefreshTokenAsync(string refreshToken)
        {
            var userId = await _refreshTokenRepository.ValidateRefreshTokenAsync(refreshToken);
            if (userId == null)
                return null;

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
                return null;

            await _refreshTokenRepository.DeleteRefreshTokenAsync(refreshToken);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecritKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:IssuerIP"],
                audience: _config["JWT:AudienceIP"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: creds
            );

            var newRefresh = await _refreshTokenRepository.GenerateRefreshTokenAsync(user.id);

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = DateTime.UtcNow.AddHours(1),
                refreshToken = newRefresh.Token,
                message = "Token refreshed successfully"
            };
        }
    }
}
