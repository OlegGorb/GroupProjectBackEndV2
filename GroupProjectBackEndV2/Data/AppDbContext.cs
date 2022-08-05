using GroupProjectBackEndV2.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupProjectBackEndV2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<StudentProgram> Programs { get; set; }
        public DbSet<TimeSpend> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentProgram>()
                .HasMany(c => c.Students)
                .WithOne(e => e.Program);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(t => t.TimeSpends)
                .WithOne(u => u.User);

        }

    }
}
