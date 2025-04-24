using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_Project.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly RentAppDbContext _context;
        public PostRepository(RentAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PostSummaryDto>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Select(p => new PostSummaryDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    //Images = p.images,
                    AccseptedStatus = p.Accsepted_Status
                })
                .ToListAsync();
        }

        public async Task<List<PostSummaryDto>> GetPendingPostsAsync()
        {
            return await _context.Posts
                .Where(p => p.Accsepted_Status == 0)
                .Select(p => new PostSummaryDto
                {
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    //Images = p.images,
                    AccseptedStatus = p.Accsepted_Status
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
    }
}
