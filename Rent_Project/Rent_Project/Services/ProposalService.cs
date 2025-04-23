using System.IO;
using Microsoft.AspNetCore.Identity;
using Rent_Project.DTO;
using Rent_Project.Model;
using Rent_Project.Repository;

namespace Rent_Project.Services
{
    public class ProposalService
    {
        private readonly ProposalRepository _proposalRepository;
        public ProposalService(ProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;

        }
        public async Task<string> AddProposal(ProposalDto dto)
        { 
           using var stream = new MemoryStream();
             await dto.Document.CopyToAsync(stream);

        var proposal = new Proposal
        {
            name = dto.name,
            Phone = dto.Phone,
            PostId = dto.PostId,
            UserId = dto.UserId,
            Document = stream.ToArray(),

        };


        await _proposalRepository.AddAsync(proposal);
            return "Done";
        }
    }
}
