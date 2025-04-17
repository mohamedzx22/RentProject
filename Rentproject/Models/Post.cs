using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentproject.Models
{
    public class Post
    {
        [Key]
        public int id { get; set; }
        public int Number_of_viewers { get; set; }
        public int rental_status { get; set; }//0-avilable 1-rent
        public string Title { get; set; }
        public string Description { get; set; }
        public int Accsepted_Status { get; set; }
        public string images { get; set; }
        public string location { get; set; }
        public int Price { get; set; }
        public int User_id { get; set; }
        public int Landlord_name { get; set; }

        [ForeignKey("LandlordId")]
        public virtual User Landlord { get; set; }
        public virtual ICollection<Save_Post> Save_Posts { get; set; }
        public virtual ICollection<Proposal> Proposals { get; set; }


    }
}
