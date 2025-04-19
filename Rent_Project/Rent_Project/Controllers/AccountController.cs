using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent_Project.DTO;
using Rent_Project.Model;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; 

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RentAppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(RentAppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!dto.Email.Contains("@"))
            {
                return BadRequest("Email must contain @");
            }
            if (dto.Role != 1 && dto.Role != 2 && dto.Role != 3)
            {
                return BadRequest("Invalid role. Must be 1 (Admin), 2 (Landlord), or 3 (Tenant).");
            }
            if (await _context.Users.AnyAsync(u => u.email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

            var user = new User
            {
                name = dto.Username,
                email = dto.Email,
                number=dto.Number,
                role= dto.Role
            };

            user.password = _passwordHasher.HashPassword(user, dto.Password);


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registered Successfully");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.Email);
            if (user == null)
            {
                return BadRequest("Email not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password");
            }

            //List<Claim> UserClaims = new List<Claim>();
            //UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            //UserClaims.Add(new ClaimsIdentity(ClaimsTypes.NameIdentifier,userFromDb.User))



            //JwtSecurityToken mytocken = new JwtSecurityToken(
            //    audience:"",
            //    issuer:""


            return Ok("Login  Successful");

        }


    }
}