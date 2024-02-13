using Microsoft.EntityFrameworkCore;
using SmartScannerBackend.Models;

namespace SmartScannerBackend.DataAccess
{
    public class SmartScannerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SmartScannerDbContext(DbContextOptions<SmartScannerDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasData(
                    new User
                    {
                        UserName = "shiningdevusername",
                        Password = "shiningdevpassword",
                        Disqualified = false,
                        Role = User.UserRole.Admin
                    }
                );
            });
        }
    }
}