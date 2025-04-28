using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rent_Project.DTO;
using Rent_Project.Migrations;
using Rent_Project.Model;
using Rent_Project.Repository;
using Microsoft.AspNetCore.Authorization;
using Rent_Project.Services;

namespace Rent_Project.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo; 
        private readonly ICurrentUserService _currentUserService;

        public PostController(IPostRepository postRepo, IUserRepository userRepo, ICurrentUserService currentUserService)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;  
            _currentUserService = currentUserService;
        }

        [HttpPost("add-1-view-post/{id}")]
        public async Task<IActionResult> ViewPost(int id)
        {
            var viewers = await _postRepo.AddViewToPostAsync(id);
            if (viewers == -1)
                return NotFound();

            return Ok(new { viewers });
        }


        [HttpGet("view-tenant")]
        public async Task<IActionResult> GetAcceptedPosts()
        {
            var posts = await _postRepo.GetAcceptedPostsAsync();
            return Ok(posts);
        }

        [Authorize(Roles = "2")]
        [HttpGet("PostDetails/{id}")]
        public async Task<ActionResult<PostDto>> GetPostById(int id)
        {
            var post = await _postRepo.GetPostDetailsByIdAsync(id);

            if (post == null)
                return NotFound($"Post with ID {id} not found");

            return Ok(post);
        }


        [HttpPost("landlord")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto postDto)
        {
            byte[] imageBytes = null;

            
            var landlord = await _userRepo.GetByIdAsync(_currentUserService.GetUserId());
            if (landlord == null)
                return NotFound("Landlord not found");

            if (postDto.Image != null && postDto.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await postDto.Image.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            var post = new Post
            {
                Title = postDto.Title,
                Description = postDto.Description,
                location = postDto.Location,
                image = imageBytes,
                Price = postDto.Price,
                rental_status = 0,
                Accsepted_Status = 0,
                Number_of_viewers = 0,
                Landlord_id = landlord.id,
                Landlord_name = landlord.name,
            };

            
            await _postRepo.AddAsync(post);

            return Ok(new
            {
                Message = "Post created successfully",
                PostId = post.id
            });
        }
    

        [Authorize(Roles = "2")]
        [HttpPatch("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostDto updatePostDto)
        {
            
            var post = await _postRepo.GetPostEntityByIdAsync(id);

            if (post == null)
                return NotFound($"Post with ID {id} not found.");

            
            if (post.Landlord_id != _currentUserService.GetUserId())
                return Forbid("You are not allowed to update this post.");

          
            if (!string.IsNullOrEmpty(updatePostDto.Title))
                post.Title = updatePostDto.Title;

            if (!string.IsNullOrEmpty(updatePostDto.Description))
                post.Description = updatePostDto.Description;

            if (!string.IsNullOrEmpty(updatePostDto.location))
                post.location = updatePostDto.location;

            if (updatePostDto.Price.HasValue)
                post.Price = (int)updatePostDto.Price;

            if (updatePostDto.images != null && updatePostDto.images.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await updatePostDto.images.CopyToAsync(ms);
                    post.image = ms.ToArray();
                }
            }


            await _postRepo.UpdateAsync(post);

            return NoContent();
        }



        [Authorize(Roles = "2")]
        [Authorize(Roles = "2")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
                return NotFound($"Post with ID {id} not found.");

            
            await _postRepo.DeleteProposalsByPostIdAsync(id);

            await _postRepo.DeleteAsync(post);

            if (post.Landlord_id != _currentUserService.GetUserId())
                return Forbid("You are not allowed to delete this post.");

            return NoContent();
        }



    }
}
