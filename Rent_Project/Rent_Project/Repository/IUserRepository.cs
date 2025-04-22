using Rent_Project.Model;

namespace Rent_Project.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}