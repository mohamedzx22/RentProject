using System.ComponentModel.DataAnnotations;

namespace Rent_Project.DTO
{
    public class AdminDto
    {
        public class LandlordDtoForAdmin
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public string Email { get; set; }
            public int? LandlordStatus { get; set; }
            public List<PostSummaryDto> Posts { get; set; }
        }

        public class LandlordActionDto
        {
            public int Id { get; set; }
        }

        public class PostSummaryDto
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public int Price { get; set; }
            public int RentalStatus { get; set; }
            public int NumberOfViewers { get; set; }
            public string LandlordName { get; set; }
            public string Images { get; set; }
            public int AccseptedStatus { get; set; }
        }

    }
}
