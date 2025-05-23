﻿using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rent_Project.Model
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int id { get; set; }

        [MaxLength(50)]
        public string name { get; set; }

        [MaxLength(200)]
        public string password { get; set; }
        public string number { get; set; }
        public string email { get; set; }
        public int role { get; set; }//1-Admin  2-Landlord  3-Tenant

        public virtual ICollection<Message> SentMessages { get; set; }

        public virtual ICollection<Message> ReceivedMessages { get; set; }
        
        public virtual ICollection<Post> Posts { get; set; }
        
        public virtual ICollection<Save_Post> Save_Posts { get; set; }
        
        public virtual ICollection<Proposal> Proposals { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

        public Landlord Landlord { get; set; }
    }
}
