using Microsoft.EntityFrameworkCore;
using MaybeFinal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MaybeFinal.Contexts
{
    public class MaybeFinalDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Accountant> Accountants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }

        public MaybeFinalDbContext(DbContextOptions<MaybeFinalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defining relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);

            // Define the relationship with Accountant properly
            modelBuilder.Entity<User>()
                .HasOne(u => u.Accountant)  // One User has one Accountant
                .WithMany(a => a.Users)    // One Accountant can have many Users
                .HasForeignKey(u => u.AccountantId)  // Foreign Key explicitly set
                .OnDelete(DeleteBehavior.Cascade);  // Use CASCADE instead of SET NULL
            modelBuilder.Entity<Accountant>()
                .HasMany(a => a.Users)  // One Accountant has many Users
                .WithOne(u => u.Accountant)  // Each User has one Accountant
                .HasForeignKey(u => u.AccountantId)  // Explicit foreign key for User
                .OnDelete(DeleteBehavior.Cascade);  // Use CASCADE instead of SET NULL

            modelBuilder.Entity<User>()
                .Property(u => u.Salary)
                .HasColumnType("decimal(18, 2)");  // Define precision and scale for Salary

            modelBuilder.Entity<Loan>()
                .Property(l => l.Amount)
                .HasColumnType("decimal(18, 2)");  // Define precision and scale for Loan Amount
        }


    }


}




