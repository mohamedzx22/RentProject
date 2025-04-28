using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;
using Rent_Project.Services;

namespace Rent_Project.Controllers
{
    public class ProposalController : ControllerBase
    {
        private readonly ProposalService _proposalService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<Post> _postRepo;
        private readonly IGenericRepository<Proposal> _proposalRepo;

        public ProposalController(ProposalService proposalService, ICurrentUserService currentUserService,
                                  IGenericRepository<Post> postRepo, IGenericRepository<Proposal> proposalRepo)
        {
            _proposalService = proposalService;
            _currentUserService = currentUserService;
            _postRepo = postRepo;
            _proposalRepo = proposalRepo;
        }

        [Authorize(Roles = "2")]
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetProposalsByPostId(int postId)
        {
            
            var post = await _postRepo.GetByIdAsync(postId);
            if (post.Landlord_id != _currentUserService.GetUserId())
                return Forbid("You are not allowed to update this post.");

            
            var proposals = await _proposalRepo.GetAllAsync();
            var postProposals = proposals.Where(p => p.PostId == postId).ToList();

            if (postProposals == null || postProposals.Count == 0)
            {
                return NotFound($"No proposals found for PostId {postId}");
            }

            var proposalsDto = postProposals.Select(p => new
            {
                p.name,
                p.Phone,
                p.PostId,
                p.UserId,
                Document = Convert.ToBase64String(p.Document)
            }).ToList();

            return Ok(proposalsDto);
        }

        [Authorize(Roles = "2")]
        [HttpPut("approve-proposal/{id}")]
        public async Task<IActionResult> ApproveProposal(int id)
        {
            var proposal = await _proposalRepo.GetByIdAsync(id);
            if (proposal == null)
                return NotFound("Proposal not found.");

            proposal.Status = 1;

            
            var post = await _postRepo.GetByIdAsync(proposal.PostId);
            if (post == null)
                return NotFound("Post associated with the proposal not found.");

            post.rental_status = 1;

            
            await _proposalRepo.UpdateAsync(proposal); 
            await _postRepo.UpdateAsync(post);

            return Ok("Proposal approved and associated Post rental status updated successfully.");
        }

        [Authorize(Roles = "2")]
        [HttpPut("reject-proposal/{id}")]
        public async Task<IActionResult> RejectProposal(int id)
        {
            var proposal = await _proposalRepo.GetByIdAsync(id); 
            if (proposal == null)
                return NotFound("Proposal not found.");

            proposal.Status = 2;

            
            var post = await _postRepo.GetByIdAsync(proposal.PostId);
            if (post == null)
                return NotFound("Post associated with the proposal not found.");

            post.rental_status = 0;

            
            await _proposalRepo.UpdateAsync(proposal); 
            await _postRepo.UpdateAsync(post); 

            return Ok("Proposal rejected successfully.");
        }

        [Authorize(Roles = "3")]
        [HttpPost("AddProposal")]
        public async Task<IActionResult> AddProposal([FromForm] ProposalDto dto)
        {
            var UserId = _currentUserService.GetUserId();
            var result = await _proposalService.AddProposal(UserId, dto.PostId, dto);
            if (result == "Done")
                return Ok("Proposal Sent Successfully" + result);
            return BadRequest(result);
        }

        [Authorize(Roles = "2")]
        [HttpGet("Pending Proposals")]
        public async Task<IActionResult> GetPendingProposals()
            => Ok(await _proposalRepo.GetAllAsync());


        [Authorize(Roles = "2")]
        [HttpGet("accepted-proposals/{postId}")]
        public async Task<IActionResult> GetAcceptedProposals(int postId)
        {
            var proposals = await _proposalRepo.GetAllAsync();
            var acceptedProposals = proposals
                .Where(p => p.PostId == postId && p.Status == 1)
                .Select(p => new
                {
                    p.name,
                    p.Phone,
                    p.PostId,
                    p.UserId,
                    Document = Convert.ToBase64String(p.Document)
                })
                .ToList();

            if (acceptedProposals == null || acceptedProposals.Count == 0)
            {
                return NotFound($"No accepted proposals found for PostId {postId}");
            }

            return Ok(acceptedProposals);
        }

        // Get all rejected proposals for a specific post
        [Authorize(Roles = "2")]
        [HttpGet("rejected-proposals/{postId}")]
        public async Task<IActionResult> GetRejectedProposals(int postId)
        {
            var proposals = await _proposalRepo.GetAllAsync();
            var rejectedProposals = proposals
                .Where(p => p.PostId == postId && p.Status == 2)
                .Select(p => new
                {
                    p.name,
                    p.Phone,
                    p.PostId,
                    p.UserId,
                    Document = Convert.ToBase64String(p.Document)
                })
                .ToList();

            if (rejectedProposals == null || rejectedProposals.Count == 0)
            {
                return NotFound($"No rejected proposals found for PostId {postId}");
            }

            return Ok(rejectedProposals);
        }

    }



}
