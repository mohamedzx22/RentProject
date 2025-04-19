using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class LoginDto
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
