using Microsoft.AspNetCore.Identity;
using Rent_Project.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;


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

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }
    }
}
    

