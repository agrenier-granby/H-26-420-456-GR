using Microsoft.EntityFrameworkCore;
using S4_B_EF.Models;

namespace S4_B_EF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Employe> Employes { get; set; }
        public DbSet<Pays> Pays { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Projet> Projets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Departement>()
                .HasMany(x => x.Employes)
                .WithOne(x => x.Departement)
                .HasForeignKey(x => x.DepartementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Departement>()
    .           Ignore(x => x.Directeur);

            modelBuilder.Entity<Employe>()
                .HasMany(x => x.ProjetsImpliques)
                .WithMany(x => x.Employes)
                .UsingEntity<EmployeProjet>();

            modelBuilder.Entity<EmployeProjet>()
              .HasKey(x => new { x.EmployeId, x.ProjetId });

            modelBuilder.Entity<EmployeProjet>()
                .Property(x => x.DateAjout)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
