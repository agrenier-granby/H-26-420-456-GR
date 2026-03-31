using IdentityRazor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityRazor.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
    }
}
