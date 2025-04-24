namespace Rent_Project.DTO
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public IFormFile Image { get; set; }  
        public int Price { get; set; }
        public int LandlordId { get; set; }
    }
}
