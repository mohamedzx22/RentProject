using Microsoft.AspNetCore.Identity;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;
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

            return "Login Successful";
        }
    }
}
