using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
         
    }
}
