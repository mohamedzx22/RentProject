using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;
using static Rent_Project.DTO.AdminDto;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RentAppDbContext _db;
        public AdminController(RentAppDbContext db)
        {
            _db = db;
        }

        [HttpGet("all-landlords")]
        public async Task<IActionResult> GetLandlords()
        {
            var landlords = await _db.Landlords
                .Include(l => l.User)
                .Select(l => new LandlordDtoForAdmin
                {
                    Id = l.User.id,
                    Name = l.User.name,
                    Email = l.User.email,
                    Number = l.User.number,
                    LandlordStatus = l.Status,
                })
                .ToListAsync();

            return Ok(landlords);
        }
        [HttpGet("pending-landlords")]
        public async Task<IActionResult> GetPendingLandlords()
        {
            var pendingLandlords = await _db.Landlords
                .Include(l => l.User)
                .Where( l => l.Status == 0)
                .Select(l => new LandlordDtoForAdmin
                {
                    Id = l.User.id,
                    Name = l.User.name,
                    Email = l.User.email,
                    Number = l.User.number,
                    LandlordStatus = l.Status,
                })
                .ToListAsync();

            return Ok(pendingLandlords);
        }

        // Get Accepted
        [HttpGet("accepted-landlords")]
        public async Task<IActionResult> GetAcceptedLandlords()
        {
            var acceptedLandlords = await _db.Landlords
                .Include(l => l.User)
                .Where(l => l.Status == 1)
                .Select(l => new LandlordDtoForAdmin
                {
                    Id = l.User.id,
                    Name = l.User.name,
                    Email = l.User.email,
                    Number = l.User.number,
                    LandlordStatus = l.Status,
                })
                .ToListAsync();

            return Ok(acceptedLandlords);
        }

        // Approve
        [HttpPut("approve-landlord")]
        public async Task<IActionResult> ApproveLandlord([FromBody] LandlordActionDto dto)
        {
            var landlord = await _db.Landlords
                 .FirstOrDefaultAsync(l => l.UserId == dto.Id);

            if (landlord == null)
                return NotFound("Landlord not found.");

            landlord.Status = 1;
            await _db.SaveChangesAsync();

            return Ok("Landlord approved successfully.");
        }

        // Reject
        [HttpPut("reject-landlord")]
        public async Task<IActionResult> RejectLandlord([FromBody] LandlordActionDto dto)
        {
            var landlord = await _db.Landlords
                .FirstOrDefaultAsync(l => l.UserId == dto.Id);

            if (landlord == null)
                return NotFound("Landlord not found.");

            landlord.Status = 2;
            await _db.SaveChangesAsync();

            return Ok("Landlord rejected successfully.");
        }

        // Delete Rejected
        [HttpDelete("delete-rejected-landlords")]
        public async Task<IActionResult> DeleteRejectedLandlords()
        {
            var rejectedLandlords = await _db.Landlords
                .Include(l => l.User)
                .Where(l => l.Status == 2)
                .ToListAsync();

            if (rejectedLandlords.Count == 0)
                return NotFound("No rejected landlords found.");

            _db.Users.RemoveRange(rejectedLandlords.Select(l => l.User));
            var deletedCount = await _db.SaveChangesAsync();

            return Ok($"{deletedCount} rejected landlord(s) deleted.");
        }
        
        [HttpGet("All-posts")]
        public async Task<IActionResult> GetPosts()
        {
            var pendingPosts = await _db.Posts
                .Select(p => new PostSummaryDto
                {
                    PostId = p.id,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    Images = p.images != null ? Convert.ToBase64String(p.images) : null,
                    AccseptedStatus = p.Accsepted_Status
                })
                .ToListAsync();

            return Ok(pendingPosts);
        }

        [HttpGet("pending-posts")]
        public async Task<IActionResult> GetPendingPosts()
        {
            var pendingPosts = await _db.Posts
                .Where(p => p.Accsepted_Status == 0)
                .Select(p => new PostSummaryDto
                {
                    PostId = p.id,
                    Title = p.Title,
                    Description = p.Description,
                    Location = p.location,
                    Price = p.Price,
                    RentalStatus = p.rental_status,
                    NumberOfViewers = p.Number_of_viewers,
                    LandlordName = p.Landlord_name,
                    Images = p.images != null ? Convert.ToBase64String(p.images) : null, 
                    AccseptedStatus = p.Accsepted_Status
                })
                .ToListAsync();

            return Ok(pendingPosts);
        }
        

        [HttpPut("approve-Posts/{id}")]
        public async Task<IActionResult> ApprovePosts(int id)
        {
            var Posts = await _db.Posts.FindAsync(id);

            if (Posts == null)
                return NotFound("Post not found .");

            Posts.Accsepted_Status = 1;
            await _db.SaveChangesAsync();

            return Ok("Post approved successfully.");
        }

        [HttpPut("reject-post/{id}")]
        public async Task<IActionResult> RejectPost(int id)
        {
            var post = await _db.Posts.FindAsync(id);

            if (post == null)
                return NotFound("Post not found.");

            post.Accsepted_Status = 2;
            await _db.SaveChangesAsync();

            return Ok("Post rejected successfully.");
        }

        [HttpDelete("delete-rejected-posts")]
        public async Task<IActionResult> DeleteRejectedPosts()
        {

            var rejectedPosts = await _db.Posts.Where(p => p.Accsepted_Status == 2).ToListAsync();

            if (rejectedPosts.Count == 0)
                return NotFound("No rejected posts found.");

            _db.Posts.RemoveRange(rejectedPosts);
            await _db.SaveChangesAsync();

            return Ok("All rejected posts deleted successfully.");
        }

    }
}
