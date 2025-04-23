using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;
namespace Rent_Project.Services
{
    public class AccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RentAppDbContext _context; // 

        public AccountService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher , RentAppDbContext context)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _context = context; 

        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
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
                role = dto.Role,
                password = _passwordHasher.HashPassword(null!, dto.Password)
            };

            await _userRepository.AddAsync(user);
            if (dto.Role == 2)
            {
                var landlord = new Landlord
                {
                    UserId = user.id,
                    Status = 0
                };

                _context.Landlords.Add(landlord);
                await _context.SaveChangesAsync(); 

            }
            return "Registered Successfully";
            
        }

        public async Task<string> LoginAsync(LoginDto dto)
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
                {
                    return "Landlord profile not found.";
                }

                if (landlord.Status == 0)
                {
                    return "Your account is under review.";
                }

                if (landlord.Status == 2)
                {
                    return "Your account has been rejected.";
                }
            }

            return "Login Successful";
        }
    }
}
