namespace Rent_Project.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> FindAsync(Func<T, bool> predicate);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
