namespace Rent_Project.DTO
{
    public class UpdatePostDto
    {
        //public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? images { get; set; }
        public string? location { get; set; }
        public int? Price { get; set; }
        public string? Landlord_name { get; set; }
    }
}
