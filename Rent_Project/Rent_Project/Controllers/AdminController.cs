// Controllers/AdminController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent_Project.DTO;
using Rent_Project.Repository;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;

        public AdminController(IUserRepository userRepo, IPostRepository postRepo)
        {
            _userRepo = userRepo;
            _postRepo = postRepo;
        }

        [HttpGet("all-landlords")]
        [Authorize]
        public async Task<IActionResult> GetLandlords()
            => Ok(await _userRepo.GetAllLandlordsAsync());

        [HttpGet("pending-landlords")]
        public async Task<IActionResult> GetPending()
            => Ok(await _userRepo.GetPendingLandlordsAsync());

        [HttpGet("accepted-landlords")]
        public async Task<IActionResult> GetAccepted()
            => Ok(await _userRepo.GetAcceptedLandlordsAsync());

        [HttpPut("approve-landlord")]
        public async Task<IActionResult> Approve([FromBody] LandlordActionDto dto)
        {
            if (!await _userRepo.ApproveLandlordAsync(dto.Id))
                return NotFound("Landlord not found.");
            return Ok("Landlord approved successfully.");
        }

        [HttpPut("reject-landlord")]
        public async Task<IActionResult> Reject([FromBody] LandlordActionDto dto)
        {
            if (!await _userRepo.RejectLandlordAsync(dto.Id))
                return NotFound("Landlord not found.");
            return Ok("Landlord rejected successfully.");
        }

        [HttpDelete("delete-rejected-landlords")]
        public async Task<IActionResult> DeleteRejected()
        {
            var count = await _userRepo.DeleteRejectedLandlordsAsync();
            if (count == 0) return NotFound("No rejected landlords found.");
            return Ok($"{count} rejected landlord(s) deleted.");
        }

        [HttpGet("all-posts")]
        public async Task<IActionResult> GetPosts()
            => Ok(await _postRepo.GetAllPostsAsync());

        [HttpGet("pending-posts")]
        public async Task<IActionResult> GetPendingPosts()
            => Ok(await _postRepo.GetPendingPostsAsync());

        [HttpPut("approve-post")]
        public async Task<IActionResult> ApprovePost([FromBody] PostActionDto dto)
        {
            if (!await _postRepo.ApprovePostAsync(dto.Id))
                return NotFound("Post not found.");
            return Ok("Post approved successfully.");
        }

        [HttpPut("reject-post")]
        public async Task<IActionResult> RejectPost([FromBody] PostActionDto dto)
        {
            if (!await _postRepo.RejectPostAsync(dto.Id))
                return NotFound("Post not found.");
            return Ok("Post rejected successfully.");
        }

        [HttpDelete("delete-rejected-posts")]
        public async Task<IActionResult> DeleteRejectedPosts()
        {
            var count = await _postRepo.DeleteRejectedPostsAsync();
            if (count == 0) return NotFound("No rejected posts found.");
            return Ok("All rejected posts deleted successfully.");
        }
    }
}
