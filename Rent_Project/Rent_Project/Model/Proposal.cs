using System.ComponentModel.DataAnnotations.Schema;

namespace Rent_Project.Model
{
    [Table("Proposals")]
    public class Proposal
    {
        public int id { get; set; }
        public string name { get; set; }

        public byte[] Document { get; set; } 

        public int Status { get; set; } // 0-Waiting, 1-accept, 2-reject

        public string Phone { get; set; } 

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post PostNum { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User UserNum { get; set; }
    }
}
