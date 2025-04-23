using Microsoft.AspNetCore.Identity;
using Rent_Project.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


namespace Rent_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly RentAppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(RentAppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> FindAsync(Func<User, bool> predicate)
        {
            return _context.Users.FirstOrDefault(predicate);
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }
        public RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges(); 

            return refreshToken;
        }
        
    }
}
    

