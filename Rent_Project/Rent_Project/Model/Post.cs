using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent_Project.Model
{
    public class Post
    {
        [Key]
        public int id { get; set; }
        public int Number_of_viewers { get; set; }
        public int rental_status { get; set; }//0-avilable 1-rent
        public string Title { get; set; }
        public string Description { get; set; }

        public int Accsepted_Status { get; set; }//0-Waiting 1-accept  2-reject 
        public string images { get; set; }
        public string location { get; set; }
        public int Price { get; set; }
        [ForeignKey("Landlord")]
        public int Landlord_id { get; set; }
        public string Landlord_name { get; set; }
        public virtual User Landlord { get; set; }
        public virtual ICollection<Save_Post> Save_Posts { get; set; }
        public virtual ICollection<Proposal> Proposals { get; set; }


    }
}
