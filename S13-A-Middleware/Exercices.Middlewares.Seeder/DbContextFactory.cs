using Exercices.Middlewares.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exercices.Middlewares.Seeder
{
    internal static class DbContextFactory
    {
        public static ApplicationDbContext CreateDbContext()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json")
                .AddJsonFile(Directory.GetCurrentDirectory() + $"/appsettings.{environment}.json", true)
                .Build();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                if (IsDevelopment())
                {
                    // N'utiliser la console qu'en développement, car c'est lent une console!
                    builder.AddConsole();
                }
            });

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(loggerFactory);

            if (IsDevelopment())
            {
                builder.EnableSensitiveDataLogging();
            }

            return new ApplicationDbContext(builder.Options);
        }

        private static bool IsDevelopment()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return environment == "Development";
        }
    }
}
