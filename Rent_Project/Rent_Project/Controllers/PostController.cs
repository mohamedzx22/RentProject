using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rent_Project.DTO;
//using Rent_Project.Migrations;
using Rent_Project.Model;
using Rent_Project.Repository;

namespace Rent_Project.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IGenericRepository<Post> _postRepo;
        private readonly RentAppDbContext _db;

        public PostController(IGenericRepository<Post> postRepo, RentAppDbContext db)
        {
            _postRepo = postRepo;
            _db = db;
        }


        [HttpPost("add-1-view-post/{id}")]
        public async Task<IActionResult> ViewPost(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null)
                return NotFound();

              post.Number_of_viewers += 1;
              await _db.SaveChangesAsync();

            return Ok(new { viewers = post.Number_of_viewers });
        }

        [HttpGet("view-tenant")]
        public async Task<IActionResult> GetAcceptedPosts()
        {
            var posts = await _db.Posts
                .Where(p => p.Accsepted_Status == 1)
                .Select(p => new
                {
                    p.Title,
                    p.Description,
                    p.location,
                    p.Price,
                    p.rental_status,
                    p.Number_of_viewers,
                    p.Landlord_name,
                    Images = p.image != null ? Convert.ToBase64String(p.image) : null
                })
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("landlordPosts/{landlordId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByLandlordId(int landlordId)
        {
            var postsFromDb = await _db.Posts
               .Where(p => p.Landlord_id == landlordId)
               .ToListAsync();

            var posts = postsFromDb.Select(p => new PostDto
            {
                Id = p.id,
                Title = p.Title,
                Description = p.Description,
                Location = p.location,
                Image = p.image != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.image)}" : null,
                Price = p.Price,
                RentalStatus = p.rental_status,
                AcceptedStatus = p.Accsepted_Status
            }).ToList();

            if (posts == null || posts.Count == 0)
               return NotFound("No posts found for this landlord.");

            return Ok(posts);
       }
        

        [HttpGet("PostDetails/{id}")]
        public async Task<ActionResult<PostDto>> GetPostById(int id)
        {
            var post = await _db.Posts
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

                   if (post == null)
                      return NotFound($"Post with ID {id} not found");

            return Ok(post);
        }

        [HttpPost("landlord/{landlordId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto postDto, int landlordId)
        {
            byte[] imageBytes = null;

            var landlord = await _db.Users.FindAsync(landlordId);
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
                Landlord_id = landlordId,
                Landlord_name = landlord.name,
            };

            await _postRepo.AddAsync(post);

            return Ok(new
            {
                Message = "Post created successfully",
                PostId = post.id
            });
        }


        [HttpPatch("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostDto updatePostDto)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
                return NotFound($"Post with ID {updatePostDto.Id} not found.");;

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

            _postRepo.UpdateAsync(post);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            var proposalsToDelete = _db.Proposals.Where(p => p.PostId == id);
            _db.Proposals.RemoveRange(proposalsToDelete);

            await _db.SaveChangesAsync();

            _postRepo.DeleteAsync(post);

            return NoContent();
        }


    }
}
