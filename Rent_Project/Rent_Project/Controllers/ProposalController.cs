using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Services;

namespace Rent_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly ProposalService _proposalService;

        public ProposalController(ProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        [HttpPost]
        public async Task<IActionResult> Addproposal([FromForm] ProposalDto dto)
        {
            var result = await _proposalService.AddProposal(dto);
            if (result == "Done")
                return Ok(result);
            return BadRequest(result);
        }


    }
}
