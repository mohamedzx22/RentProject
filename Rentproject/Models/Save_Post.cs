using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentproject.Models
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
