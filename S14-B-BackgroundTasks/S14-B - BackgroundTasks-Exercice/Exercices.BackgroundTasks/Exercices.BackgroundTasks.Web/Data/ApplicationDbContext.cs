using Exercices.BackgroundTasks.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.BackgroundTasks.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}
