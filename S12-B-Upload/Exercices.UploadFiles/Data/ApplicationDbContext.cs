using Exercices.UploadFiles.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.UploadFiles.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}
