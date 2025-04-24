using Microsoft.AspNetCore.Authorization;
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
        private readonly ICurrentUserService _currentUserService;

        public ProposalController(ProposalService proposalService, ICurrentUserService currentUserService)
        {
            _proposalService = proposalService;
            _currentUserService = currentUserService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Addproposal(int PostId, [FromForm] ProposalDto dto)
        {
            var UserId = _currentUserService.GetUserId();
            var result = await _proposalService.AddProposal(UserId, PostId, dto);
            if (result == "Done")
                return Ok("Proposal Sent Successfuly" + result);
            return BadRequest(result);
        }


    }
}