using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection; // Added for Assembly.GetExecutingAssembly().Location

namespace Core.Data
{
    public class ShopMeDbContextFactory : IDesignTimeDbContextFactory<ShopMeDbContext>
    {
        public ShopMeDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            if (string.IsNullOrEmpty(basePath) || !File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                while (currentDirectory != null && !File.Exists(Path.Combine(currentDirectory, "Core.csproj")))
                {
                    currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
                }
                basePath = currentDirectory ?? Directory.GetCurrentDirectory();
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ShopMeDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection connection string not found in appsettings.json.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new ShopMeDbContext(optionsBuilder.Options);
        }
    }
}
