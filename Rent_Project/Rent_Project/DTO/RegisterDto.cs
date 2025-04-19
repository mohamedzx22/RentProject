using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Number {  get; set; }
        [Required]

        [Range(1, 3, ErrorMessage = "Role must be 1 (Admin), 2 (Landlord), or 3 (Tenant)")]
        public int Role { get; set; }//1-Admin  2-Landlord  3-Tenant

    }
}
