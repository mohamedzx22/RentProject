using Rent_Project.DTO;
using Rent_Project.Model;

namespace Rent_Project.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<List<LandlordDtoForAdmin>> GetAllLandlordsAsync();
        Task<List<LandlordDtoForAdmin>> GetPendingLandlordsAsync();
        Task<List<LandlordDtoForAdmin>> GetAcceptedLandlordsAsync();
        Task<bool> ApproveLandlordAsync(int landlordUserId);
        Task<bool> RejectLandlordAsync(int landlordUserId);
        Task<int> DeleteRejectedLandlordsAsync();
    }
}