using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent_Project.Model
{
    [Table("Masseges")]
    public class Message
    {
        [Key]
        public int id { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }

        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

      
    }
}
