using BackgroundTasks.Web.Exemple.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTasks.Web.Exemple.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasDefaultValueSql("getutcdate()");
        }

        public DbSet<User> Users { get; set; } = default!;

    }
}
