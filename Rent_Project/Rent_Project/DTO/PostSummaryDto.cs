// DTOs/PostSummaryDto.cs
namespace Rent_Project.DTO
{
    public class PostSummaryDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public int RentalStatus { get; set; }
        public int NumberOfViewers { get; set; }
        public string LandlordName { get; set; }
        public string? Images { get; set; }
        public int AccseptedStatus { get; set; }
    }
}
