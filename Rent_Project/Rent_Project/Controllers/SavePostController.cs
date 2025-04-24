using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;
using Rent_Project.Repository;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavePostController : ControllerBase
    {
        private readonly IGenericRepository<Save_Post> _savePostRepository;
        private readonly IGenericRepository<Post> _postRepository;
        private readonly RentAppDbContext _context;

        public SavePostController(IGenericRepository<Save_Post> savePostRepository, RentAppDbContext context)
        {
            _savePostRepository = savePostRepository;
            _context = context;
        }



        [HttpPost("save")]
        public async Task<IActionResult> SavePost(int TenantID, int PostId)
        {
            try
            {
                
                if (TenantID == null)
                {
                    return Unauthorized("User is not logged in.");
                }

                var post = await _context.Posts.FindAsync(PostId); 
                if (post == null)
                {
                    return NotFound("Post not found.");
                }

                
                var savePost = new Save_Post
                {
                    UserId = TenantID,
                    PostId = PostId
                };

                
                _context.Save_Posts.Add(savePost);

                await _context.SaveChangesAsync(); 

                return Ok("Post saved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    

    [HttpGet("saved-posts/{userId}")]
        public async Task<IActionResult> GetSavedPosts(int userId)
        {
            try
            {
                
                var savedPosts = await _context.Save_Posts
                                               .Where(sp => sp.UserId == userId)
                                               .Include(sp => sp.Post)  
                                               .ToListAsync();

                
                var userSavedPosts = savedPosts.Select(sp => new
                {
                    sp.Post.id,
                    sp.Post.Title,
                    sp.Post.Description,
                    sp.Post.Price,
                    sp.Post.location,
                    sp.Post.Number_of_viewers,
                    sp.Post.rental_status,
                    //sp.Post.images
                }).ToList();

                return Ok(userSavedPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
