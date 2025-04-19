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
    

    }
}
