using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
namespace Rent_Project.Services
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RentAppDbContext _context;
        private readonly IConfiguration config;

        public AccountService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, RentAppDbContext context, IConfiguration config)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _context = context;
            this.config = config;

        }

        public async Task<object> RegisterAsync(RegisterDto dto)
        {
            if (!dto.Email.Contains("@"))
                return "Email must contain @";


            if (dto.Password != dto.ConfirmPassword)
            {
                return "Password  Doesn't  Match";
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return "Email already exists";
            }

            var user = new User
            {
                name = dto.Username,
                email = dto.Email,
                number = dto.Number,

                password = _passwordHasher.HashPassword(null!, dto.Password),
                role = dto.Role switch
                {
                    "Admin" => 1,
                    "Landlord" => 2,
                    "Tenant" => 3,
                    _ => throw new ArgumentException("Invalid role provided") // √Ê » ÕœÌœ ﬁÌ„… «› —«÷Ì…
                }
            };
            await _userRepository.AddAsync(user);
            await _context.Entry(user).ReloadAsync(); //  √ﬂœ ≈‰ ID  ⁄„· ·Â  ÕœÌÀ

            if (user.role == 2)
            {
                var landlord = new Landlord
                {
                    UserId = user.id,
                    Status = 0
                };

                _context.Landlords.Add(landlord);
                await _context.SaveChangesAsync();
            }

            return user;

        }


        public async Task<object> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return "Email not found";
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return "Invalid password";
            }
            if (user.role == 2)
            {
                var landlord = await _context.Landlords.FirstOrDefaultAsync(l => l.UserId == user.id);
                if (landlord == null)
                    return "Landlord profile not found.";

                if (landlord.Status == 0)
                    return "Your account is under review.";

                if (landlord.Status == 2)
                    return "Your account has been rejected.";
            }
            var claims = new[]
       {
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };
            //key
            var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));

            SigningCredentials signingCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

            //design token
            JwtSecurityToken mytoken = new JwtSecurityToken(
                audience: config["JWT:AudienceIP"],
                issuer: config["JWT:IssuerIP"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: signingCred
                );
            var refreshtoken = _userRepository.GenerateRefreshToken(user.id);

            // generate token

            return new
            {

                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                expiration = DateTime.Now.AddHours(1),
                refreshtoken = refreshtoken.Token,
                role = user.role,
                message = "Login Successfully"

            };

        }
        public async Task<object> LogoutAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (tokenEntity == null) return "Unauthorized";

            _context.RefreshTokens.Remove(tokenEntity);
            await _context.SaveChangesAsync();

            return new { message = "Logged out successfully" };
        }

    }
}
