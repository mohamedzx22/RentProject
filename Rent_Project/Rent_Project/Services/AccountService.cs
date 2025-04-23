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
namespace Rent_Project.Services
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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
                
                password = _passwordHasher.HashPassword(null!, dto.Password)
            };
            if(dto.Role=="Admine")
            {
                user.role = 1;
            }else
                if (dto.Role == "Landlord")
            {
                user.role = 2;
            }
            else
            if(dto.Role == "Tenant")
            {
                user.role = 3;
            }

            await _userRepository.AddAsync(user);
            var claims = new[]
      {
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };
            //key
            var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsMySuperSecretKey1234567890!@#"));

            SigningCredentials signingCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

            //design token
            JwtSecurityToken mytoken = new JwtSecurityToken(
                audience: "http://localhost:4200/",
                issuer: "https://localhost:7101/",
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: signingCred

                );
            var refreshtoken = _userRepository.GenerateRefreshToken(user.id);
            // _context.RefreshTokens.Add(refreshToken);
             //_context.SaveChanges();
            //generate token

            return new
            {

                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                expiration = DateTime.Now.AddHours(1),
                refreshtoken = refreshtoken.Token

            };
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
            var claims = new[]
       {
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.Role, user.role.ToString())
        };
            //key
            var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsMySuperSecretKey1234567890!@#$"));

            SigningCredentials signingCred = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

            //design token
            JwtSecurityToken mytoken = new JwtSecurityToken(
                audience: "http://localhost:4200/",
                issuer: "https://localhost:7101/",
                expires:DateTime.Now.AddHours(1),
                claims:claims,
                signingCredentials:signingCred

                );
            var refreshtoken = _userRepository.GenerateRefreshToken(user.id);
            //_context.RefreshTokens.Add(refreshToken);
            //_context.SaveChanges();
            //generate token

            return new
            {
                
                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                expiration = DateTime.Now.AddHours(1),
                refreshtoken=refreshtoken.Token


            };
            //return "Login Successful";
        }
    }
}
