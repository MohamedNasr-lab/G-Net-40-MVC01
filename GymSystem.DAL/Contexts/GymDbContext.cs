
using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymSystem.DAL.Contexts
{
    public class GymDbContext : DbContext
    {
        
       
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}