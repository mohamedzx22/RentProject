using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Rent_Project.Model
{
    [Table("Landlords")]
    public class Landlord

    {
            [Key]
            public int Id { get; set; }
            [ForeignKey("User")]
            public int UserId { get; set; }
            public User User { get; set; }

        public int Status { get; set; } // 0:wait 1:accept 2:reject

    }
}
