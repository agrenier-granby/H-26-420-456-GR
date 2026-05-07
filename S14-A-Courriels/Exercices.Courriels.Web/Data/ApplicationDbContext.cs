using Exercices.Courriels.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.Courriels.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>()
                .Property(s => s.RegistrationDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()")
                .HasComment("Indique la date/heure de son inscription");
        }

        public DbSet<Subscriber> Subscribers { get; set; }

    }
}
