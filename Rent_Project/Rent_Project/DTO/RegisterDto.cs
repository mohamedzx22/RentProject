using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
         
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        public string Number {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]

       
        public string Role { get; set; }//1-Admin  2-Landlord  3-Tenant

    }
}
