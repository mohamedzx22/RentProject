using Rent_Project.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent_Project.DTO
{
    public class ProposalDto
    {
        
        public string name { get; set; }
        public int Phone { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public IFormFile Document { get; set; }
        
        


    }
}
