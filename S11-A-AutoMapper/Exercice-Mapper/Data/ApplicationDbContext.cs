using Exercice_Mapper.Data.Configuration;
using Exercice_Mapper.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercice_Mapper.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaysConfiguration());
        }

        public DbSet<Pays> Pays { get; set; } = default!;
        public DbSet<Departement> Departements { get; set; } = default!;
        public DbSet<Employe> Employes { get; set; } = default!;
    }
}