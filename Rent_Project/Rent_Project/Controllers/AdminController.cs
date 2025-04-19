using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;

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

        [HttpGet("pending-landlords")]
        public async Task<IActionResult> GetPendingLandlords()
        {
            var pendingLandlords = await _db.Users
                .Where(u => u.role == 2 && u.Landlord_Status == 0)
                .Select(u => new {
                    u.id,
                    u.name,
                    u.email,
                    u.number
                })
               .ToListAsync();

            return Ok(pendingLandlords);
        }


        [HttpPost("approve-landlord/{id}")]
        public async Task<IActionResult> ApproveLandlord(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null || user.role != 2)
                return NotFound("User not found or not a landlord.");

            user.Landlord_Status = 1;
            await _db.SaveChangesAsync();

            return Ok("Landlord approved successfully.");
        }

        [HttpPost("reject-landlord/{id}")]
        public async Task<IActionResult> RejectLandlord(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if (user == null || user.role != 2)
                return NotFound("User not found or not a landlord.");

            user.Landlord_Status = 2;
            await _db.SaveChangesAsync();

            return Ok("Landlord rejected successfully.");
        }

        [HttpDelete("delete-rejected-landlords")]
        public async Task<IActionResult> DeleteRejectedLandlords()
        {
            var rejectedLandlords = await _db.Users
                .Where(u => u.role == 2 && u.Landlord_Status == 2)
                .ToListAsync();

            if (!rejectedLandlords.Any())
                return NotFound("No rejected landlords found.");

            _db.Users.RemoveRange(rejectedLandlords);
            await _db.SaveChangesAsync();

            return Ok($"{rejectedLandlords.Count} rejected landlord(s) deleted.");
        }

        [HttpGet("pending-Posts")]
        public async Task<IActionResult> GetPendingPosts()
        {
            var pendingPosts = await _db.Posts
                .Where(L => L.Accsepted_Status == 0)
                .Select(L => new
                {
                    L.Title,
                    L.Description,
                    L.location,
                    L.Price,
                    L.rental_status,
                    L.Number_of_viewers,
                    L.Landlord_name,
                    L.images,
                })
                .ToListAsync();

            return Ok(pendingPosts);
        }

        [HttpPost("approve-Posts/{id}")]
        public async Task<IActionResult> ApprovePosts(int id)
        {
            var Posts = await _db.Posts.FindAsync(id);

            if (Posts == null)
                return NotFound("Post not found .");

            Posts.Accsepted_Status = 1;
            await _db.SaveChangesAsync();

            return Ok("Post approved successfully.");
        }

        [HttpPost("reject-post/{id}")]
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
