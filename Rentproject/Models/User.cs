using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentproject.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int id { get; set; }

        [MaxLength(50)]
        public string name { get; set; }
        public int password { get; set; }
        public int number { get; set; }
        public string email { get; set; }
        public int role { get; set; }//1-Admin  2-Landlord  3-Tenant

        public virtual ICollection<Message> SentMessages { get; set; }

        public virtual ICollection<Message> ReceivedMessages { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Save_Post> Save_Posts { get; set; }
        public virtual ICollection<Proposal> Proposals { get; set; }

    }
}
