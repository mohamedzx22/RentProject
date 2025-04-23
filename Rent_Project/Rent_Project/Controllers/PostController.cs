//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using Rent_Project.DTO;
//using Rent_Project.Migrations;
//using Rent_Project.Model;

//namespace Rent_Project.Controllers
//{
    
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PostController : ControllerBase
//    {
//        private readonly RentAppDbContext _db;

//        public PostController(RentAppDbContext db)
//        {
//            _db = db;
//        }

//        [HttpPost("add-1-view-post/{id}")]
//        public async Task<IActionResult> ViewPost(int id)
//        {
//            var post = await _db.Posts.FindAsync(id);
//            if (post == null)
//                return NotFound();

//            post.Number_of_viewers += 1;
//            await _db.SaveChangesAsync();

//            return Ok(new { viewers = post.Number_of_viewers });
//        }
        
//        [HttpGet("view-tenant")]
//        public async Task<IActionResult> GetAcceptedPosts()
//        {
//            var posts = await _db.Posts
//                .Where(p => p.Accsepted_Status == 1)
//                .Select(p => new
//                {
//                    p.Title,
//                    p.Description,
//                    p.location,
//                    p.Price,
//                    p.rental_status,
//                    p.Number_of_viewers,
//                    p.Landlord_name,
//                    Images = p.images != null ? Convert.ToBase64String(p.images) : null
//                })
//                .ToListAsync();

//            return Ok(posts);
//        }
        
//        [HttpGet("landlordPosts/{landlordId}")]
//        public async Task<ActionResult<IEnumerable<PostSummaryDto>>> GetPostsByLandlordId(int landlordId)
//        {
//            var posts = await _db.Posts
//                .Where(p => p.Landlord_id == landlordId)
//                .Select(p => new PostSummaryDto
//                {
//                    Id = p.id,
//                    Title = p.Title,
//                    Description = p.Description,
//                    Location = p.location,
//                    Images = p.images != null ? Convert.ToBase64String(p.images) : null,
//                    Price = p.Price,
//                    RentalStatus = p.rental_status,
//                    AcceptedStatus = p.Accsepted_Status
//                })
//                .ToListAsync();

//            if (posts == null || posts.Count == 0)
//                return NotFound("No posts found for this landlord.");

//            return Ok(posts);
//        }
        
//        [HttpGet("PostDetails/{id}")]
//        public async Task<ActionResult<PostDto>> GetPostById(int id)
//        {
//            var post = await _db.Posts
//                .Where(p => p.id == id)
//                .Select(p => new PostDto
//                {
//                    Id = p.id,
//                    Title = p.Title,
//                    Description = p.Description,
//                    Location = p.location,
//                    Images = p.images != null ? Convert.ToBase64String(p.images) : null,
//                    Price = p.Price,
//                    RentalStatus = p.rental_status,
//                    AcceptedStatus = p.Accsepted_Status
//                })
//                .FirstOrDefaultAsync();

//            if (post == null)
//                return NotFound($"Post with ID {id} not found");

//            return Ok(post);
//        }
        
//        [HttpPost("landlord/{landlordId}")]
//        public async Task<IActionResult> AddPost(
//            string PostTitle,
//            string PostDescription,
//            IFormFile PostImage,
//            string PostLocation,
//            int PostPrice,
//            int landlordId)
//        {
//            var landlord = await _db.Users.FindAsync(landlordId);
//            if (landlord == null)
//                return NotFound("Landlord not found");

//            byte[] imageData = null;
//            if (PostImage != null)
//            {
//                using var ms = new MemoryStream();
//                await PostImage.CopyToAsync(ms);
//                imageData = ms.ToArray();
//            }

//            var newPost = new Post
//            {
//                Title = PostTitle,
//                Description = PostDescription,
//                images = imageData,
//                location = PostLocation,
//                Price = PostPrice,
//                Landlord_id = landlordId,
//                Number_of_viewers = 0,
//                Landlord_name = landlord.name
//            };

//            await _db.Posts.AddAsync(newPost);
//            await _db.SaveChangesAsync();

//            return Ok(new { postId = newPost.id });
//        }
        
//        [HttpPatch("update")]
//        public async Task<IActionResult> UpdatePost(UpdatePostDto updatedPost)
//        {
//            var p = await _db.Posts.FindAsync(updatedPost.Id);
//            if (p == null)
//                return NotFound($"Post with ID {updatedPost.Id} not found.");

//            if (updatedPost.Title != null) p.Title = updatedPost.Title;
//            if (updatedPost.Description != null) p.Description = updatedPost.Description;
//            if (updatedPost.images != null)
//            {
//                using var ms = new MemoryStream();
//                await updatedPost.images.CopyToAsync(ms);
//                p.images = ms.ToArray();
//            }
//            if (updatedPost.location != null) p.location = updatedPost.location;
//            if (updatedPost.Price.HasValue) p.Price = updatedPost.Price.Value;
//            if (updatedPost.Landlord_name != null) p.Landlord_name = updatedPost.Landlord_name;

//            await _db.SaveChangesAsync();
//            return Ok(p);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> RemovePost(int id)
//        {
//            var p = await _db.Posts.FindAsync(id);
//            if (p == null)
//                return NotFound($"Post with ID {id} not found.");

//            _db.Posts.Remove(p);
//            await _db.SaveChangesAsync();
//            return Ok(p);
//        }
        
//    }

//}
