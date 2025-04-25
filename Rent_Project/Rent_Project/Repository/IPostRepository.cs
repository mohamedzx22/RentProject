using Rent_Project.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rent_Project.Repository
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPostsAsync();
        Task<List<PostDto>> GetPendingPostsAsync();
        Task<bool> ApprovePostAsync(int postId);
        Task<bool> RejectPostAsync(int postId);
        Task<int> DeleteRejectedPostsAsync();
    }
}
