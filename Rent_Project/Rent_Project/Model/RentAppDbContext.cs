using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Rent_Project.Model
{
    public class RentAppDbContext : DbContext
    {
        public RentAppDbContext(DbContextOptions<RentAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messeges { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Save_Post> Save_Posts { get; set; }
        public DbSet<Proposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .Property(m => m.rental_status)
                .HasDefaultValue(0);

            modelBuilder.Entity<Save_Post>()
                .HasKey(sp => new { sp.PostId, sp.UserId });

            modelBuilder.Entity<Save_Post>()
                .HasOne(sp => sp.Rentant)
                .WithMany(u => u.Save_Posts)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Save_Post>()
                .HasOne(sp => sp.Post)
                .WithMany(p => p.Save_Posts)
                .HasForeignKey(sp => sp.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.PostNum)
                .WithMany(u => u.Proposals)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proposal>()
                .HasOne(p => p.UserNum)
                .WithMany(u => u.Proposals)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
               .Property(u => u.Landlord_Status)
               .HasDefaultValue(0);

            modelBuilder.Entity<Post>()
               .Property(u => u.Accsepted_Status)
               .HasDefaultValue(0);

            modelBuilder.Entity<Proposal>()
               .Property(u => u.Status)
               .HasDefaultValue(0);
        }
    }

}
