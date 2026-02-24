using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using S5_A_SEEDER.Data;

namespace S5_A_SEEDER.Seeder
{
    public class DbContextFactory
    {
        public static ApplicationDbContext CreateDbContext()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("Exercice3_ModeleEFCoreDBContext");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }

    }
}
