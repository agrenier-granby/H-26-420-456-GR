using Exercices_Formulaire_Web.Data.Configuration;
using Exercices_Formulaire_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices_Formulaire_Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaysConfiguration());
        }

        public DbSet<Pays> Pays { get; set; } = default!;
        public DbSet<Departement> Departements { get; set; } = default!;
        public DbSet<Employe> Employes { get; set; } = default!;
    }


}