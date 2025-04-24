using Microsoft.AspNetCore.Identity;
using Rent_Project.Model;
using Microsoft.EntityFrameworkCore;

namespace Rent_Project.Repository
{
    public class ProposalRepository : IGenericRepository<Proposal>
    {
        private readonly RentAppDbContext _context;

        public ProposalRepository(RentAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync()
        {
            return await _context.Proposals.ToListAsync();
        }

        public async Task<Proposal> GetByIdAsync(int id)
        {
            return await _context.Proposals.FindAsync(id);
        }

        public async Task<Proposal> FindAsync(Func<Proposal, bool> predicate)
        {
            return _context.Proposals.FirstOrDefault(predicate);
        }

        public async Task AddAsync(Proposal entity)
        {
            await _context.Proposals.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proposal entity)
        {
            _context.Proposals.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Proposal entity)
        {
            _context.Proposals.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}
