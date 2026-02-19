using Microsoft.EntityFrameworkCore;
using S4_A_EF.Models;

namespace S4_A_EF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Employe> Employes { get; set; }
        public DbSet<Pays> Pays { get; set; }
    }
}
