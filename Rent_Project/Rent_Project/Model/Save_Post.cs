using System.ComponentModel.DataAnnotations.Schema;

namespace Rent_Project.Model
{
    public class Save_Post
    {

        public int PostId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User Rentant { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }


    }
}
