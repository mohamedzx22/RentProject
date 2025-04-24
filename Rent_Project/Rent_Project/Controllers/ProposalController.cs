using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly RentAppDbContext _context;
        public ProposalController(RentAppDbContext db) {

            _context = db;
        }
        [HttpPost]
        public async Task<IActionResult>AddProposal([FromForm]ProposalDto p )
        {
            using var stream = new MemoryStream();
            await p.Document.CopyToAsync( stream );

            var proposal = new Proposal
            {
                name = p.name,
                Phone = p.Phone,
                PostId = p.PostId,
                UserId = p.UserId,
                Document= stream.ToArray()
            };
            await _context.AddAsync(proposal);
            await _context.SaveChangesAsync();
            return Ok(proposal);


        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetProposalsByPostId(int postId)
        {
            var proposals = await _context.Proposals
                .Where(p => p.PostId == postId)
                .ToListAsync();

            if (proposals == null || proposals.Count == 0)
            {
                return NotFound($"No proposals found for PostId {postId}");
            }

            var proposalsDto = proposals.Select(p => new
            {
                p.name,
                p.Phone,
                p.PostId,
                p.UserId,
                Document = Convert.ToBase64String(p.Document)  
            }).ToList();

            return Ok(proposalsDto); 
        }

        [HttpPut("approve-proposal/{id}")]
        public async Task<IActionResult> ApproveProposal(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);

            if (proposal == null)
                return NotFound("Proposal not found.");

            proposal.Status = 1;

            var post = await _context.Posts.FindAsync(proposal.PostId);

            if (post == null)
                return NotFound("Post associated with the proposal not found.");

            post.rental_status = 1;

            await _context.SaveChangesAsync();

            return Ok("Proposal approved and associated Post rental status updated successfully.");
        }


        [HttpPut("reject-proposal/{id}")]
        public async Task<IActionResult> RejectProposal(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);

            if (proposal == null)
                return NotFound("Proposal not found.");
            
            proposal.Status = 2; 

            await _context.SaveChangesAsync();

            return Ok("Proposal rejected successfully.");
        }



    }
}
