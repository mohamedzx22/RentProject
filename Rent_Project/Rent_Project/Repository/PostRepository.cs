using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_Project.Repository
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly RentAppDbContext _context;
        public PostRepository(RentAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Select(p => new PostDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    Image = p.image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.image)}" : null,
                    AcceptedStatus = p.Accsepted_Status
                })
                .ToListAsync();
        }

        public async Task<List<PostDto>> GetPendingPostsAsync()
        {
            return await _context.Posts
                .Where(p => p.Accsepted_Status == 0)
                .Select(p => new PostDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    Image = p.image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.image)}" : null,
                    AcceptedStatus = p.Accsepted_Status
                })
                .ToListAsync();
        }

        public async Task<bool> ApprovePostAsync(int postId)
        {
            var p = await _context.Posts.FindAsync(postId);
            if (p == null) return false;
            p.Accsepted_Status = 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectPostAsync(int postId)
        {
            var p = await _context.Posts.FindAsync(postId);
            if (p == null) return false;
            p.Accsepted_Status = 2;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> DeleteRejectedPostsAsync()
        {
            var rejected = await _context.Posts
                .Where(p => p.Accsepted_Status == 2)
                .ToListAsync();
            if (!rejected.Any()) return 0;
            _context.Posts.RemoveRange(rejected);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddViewToPostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return -1;

            post.Number_of_viewers += 1;
            await _context.SaveChangesAsync();
            return post.Number_of_viewers;
        }

        public async Task<List<PostDto>> GetAcceptedPostsAsync()
        {
            return await _context.Posts
                .Where(p => p.Accsepted_Status == 1)
                .Select(p => new PostDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    Image = p.image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.image)}" : null,
                    AcceptedStatus = p.Accsepted_Status
                })
                .ToListAsync();
        }

        public async Task<PostDto> GetPostDetailsByIdAsync(int id)
        {
            return await _context.Posts
                .Where(p => p.id == id)
                .Select(p => new PostDto
                {
                    Id = p.id,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Image = p.image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.image)}" : null,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    AcceptedStatus = p.Accsepted_Status
                })
                .FirstOrDefaultAsync();
        }
        public async Task<Post> GetPostEntityByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task DeleteProposalsByPostIdAsync(int postId)
        {
            var proposalsToDelete = await _context.Proposals
                .Where(p => p.PostId == postId)
                .ToListAsync();

            if (proposalsToDelete.Any())
            {
                _context.Proposals.RemoveRange(proposalsToDelete);
                await _context.SaveChangesAsync();
            }


        }
    }
}
