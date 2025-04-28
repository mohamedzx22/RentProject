using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.Model;
using Rent_Project.Repository;
using Rent_Project.Services;

namespace Rent_Project.Controllers
{
    [Authorize(Roles = "3")]
    [ApiController]
    [Route("api/[controller]")]
    public class SavePostController : ControllerBase
    {
        private readonly IGenericRepository<Save_Post> _savePostRepo;
        private readonly IGenericRepository<Post> _postRepo;
        private readonly ICurrentUserService _currentUserService;

        public SavePostController(IGenericRepository<Save_Post> savePostRepo, IGenericRepository<Post> postRepo, ICurrentUserService currentUserService)
        {
            _savePostRepo = savePostRepo;
            _postRepo = postRepo;
            _currentUserService = currentUserService;
        }

        [HttpGet("saved-posts/{userId}")]
        public async Task<IActionResult> GetSavedPosts(int userId)
        {
            try
            {
                
                var savedPosts = await _savePostRepo.GetAllAsync();
                var userSavedPosts = savedPosts
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => sp.PostId)
                    .ToList();

                if (userSavedPosts.Count == 0)
                    return NotFound("No saved posts found for this user.");

                
                var posts = new List<object>();
                foreach (var postId in userSavedPosts)
                {
                    var post = await _postRepo.GetByIdAsync(postId);
                    if (post != null)
                    {
                        posts.Add(new
                        {
                            post.id,
                            post.Title,
                            post.Description,
                            post.Price,
                            post.location,
                            post.Number_of_viewers,
                            post.rental_status,
                            post.image
                        });
                    }
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-post")]
        public async Task<IActionResult> SavePost(int postId)
        {
            var userId = _currentUserService.GetUserId();
            var post = await _postRepo.GetByIdAsync(postId);

            if (post == null)
                return NotFound("Post not found.");

            
            var savedPost = new Save_Post { UserId = userId, PostId = postId };
            await _savePostRepo.AddAsync(savedPost);

            return Ok("Post saved successfully.");
        }

        [HttpDelete("unsave-post/{postId}")]
        public async Task<IActionResult> UnsavePost(int postId)
        {
            var userId = _currentUserService.GetUserId();
            var savedPost = await _savePostRepo.FindAsync(sp => sp.UserId == userId && sp.PostId == postId);

            if (savedPost == null)
                return NotFound("Saved post not found.");

            await _savePostRepo.DeleteAsync(savedPost);

            return Ok("Post unsaved successfully.");
        }
    }
}
