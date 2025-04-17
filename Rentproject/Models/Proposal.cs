using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentproject.Models
{
    [Table("Proposals")]
    public class Proposal
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Document { get; set; }
        public int Status { get; set; }//0-accept  1-reject
        public int Phone { get; set; }
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post PostNum { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User UserNum { get; set; }


    }
}
