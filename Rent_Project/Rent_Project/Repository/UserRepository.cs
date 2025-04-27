using Microsoft.AspNetCore.Identity;
using Rent_Project.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
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


        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<List<LandlordDtoForAdmin>> GetAllLandlordsAsync()
        {
            return await _context.Users
                .Include(u => u.Landlord)
                .Where(u => u.Landlord != null)
                .Select(u => new LandlordDtoForAdmin
                {
                    Name = u.name,
                    Email = u.email,
                    LandlordStatus = u.Landlord.Status
                })
                .ToListAsync();
        }

        public async Task<List<LandlordDtoForAdmin>> GetPendingLandlordsAsync()
        {
            return await _context.Users
                .Include(u => u.Landlord)
                .Where(u => u.Landlord != null && u.Landlord.Status == 0)
                .Select(u => new LandlordDtoForAdmin
                {
                    Name = u.name,
                    Email = u.email,
                    LandlordStatus = u.Landlord.Status
                })
                .ToListAsync();
        }

        public async Task<List<LandlordDtoForAdmin>> GetAcceptedLandlordsAsync()
        {
            return await _context.Users
                .Include(u => u.Landlord)
                .Where(u => u.Landlord != null && u.Landlord.Status == 1)
                .Select(u => new LandlordDtoForAdmin
                {
                    Name = u.name,
                    Email = u.email,
                    LandlordStatus = u.Landlord.Status
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveLandlordAsync(int landlordUserId)
        {
            var landlord = await _context.Landlords
                .FirstOrDefaultAsync(l => l.UserId == landlordUserId);
            if (landlord == null) return false;
            landlord.Status = 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectLandlordAsync(int landlordUserId)
        {
            var landlord = await _context.Landlords
                .FirstOrDefaultAsync(l => l.UserId == landlordUserId);
            if (landlord == null) return false;
            landlord.Status = 2;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteRejectedLandlordsAsync()
        {
            var rejected = await _context.Landlords
                .Include(l => l.User)
                .Where(l => l.Status == 2)
                .ToListAsync();
            if (!rejected.Any()) return 0;
            _context.Users.RemoveRange(rejected.Select(l => l.User));
            return await _context.SaveChangesAsync();
        }

     
         
       


 
    }
}
    

    

