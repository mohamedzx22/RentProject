using Rent_Project.DTO;
using Rent_Project.Model;
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
        Task<int> AddViewToPostAsync(int id);
        Task<List<PostDto>> GetAcceptedPostsAsync();
        Task<PostDto> GetPostDetailsByIdAsync(int id);
        Task<Post> GetPostEntityByIdAsync(int id);
        Task DeleteProposalsByPostIdAsync(int postId);
        Task UpdateAsync(Post entity);
        Task AddAsync(Post entity);
        Task DeleteAsync(Post entity);
        Task<Post> GetByIdAsync(int id);

    }
}
