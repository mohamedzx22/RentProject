using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;

namespace Rent_Project.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RentAppDbContext _context;
        private readonly DbSet<T> _table;

        public GenericRepository(RentAppDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _table.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _table.FindAsync(id);

        public async Task<T> FindAsync(Func<T, bool> predicate)
        {
            return await Task.FromResult(_table.AsEnumerable().FirstOrDefault(predicate));
        }

        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task  Update(T entity)
        {
            _table.Update(entity);
            _context.SaveChanges();
        }

        public async Task Delete(T entity)
        {
            _table.Remove(entity);
            _context.SaveChanges();
        }
    }
}
