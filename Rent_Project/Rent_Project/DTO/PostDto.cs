namespace Rent_Project.DTO
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int RentalStatus { get; set; }
        public int AcceptedStatus {  get; set; }
        
    }
}
