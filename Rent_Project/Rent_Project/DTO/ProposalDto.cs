using Rent_Project.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent_Project.DTO
{
    public class ProposalDto
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public IFormFile Document { get; set; }
        
        


    }
}
