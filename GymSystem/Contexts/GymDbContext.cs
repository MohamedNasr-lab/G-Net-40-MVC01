using GymSystem.Configurations;
using Microsoft.EntityFrameworkCore;
namespace GymSystem.Contexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-PS1NMJJ\\SQLEXPRESS;database=GymDb;trusted_connection=true;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfigurations());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
