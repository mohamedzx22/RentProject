using System.ComponentModel.DataAnnotations.Schema;

namespace Rent_Project.Model
{
    [Table("Proposals")]
    public class Proposal
    {
        public int id { get; set; }
        public string name { get; set; }
<<<<<<< HEAD
        public string Document { get; set; }
        public int Status { get; set; } //0-Waiting 1-accept  2-reject
=======
        public byte[] Document { get; set; }
        public int Status { get; set; }//0-accept  1-reject
>>>>>>> 56cf429a6e5be686436d3e32ee68dbca3266a30f
        public int Phone { get; set; }
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post PostNum { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User UserNum { get; set; }


    }
}
