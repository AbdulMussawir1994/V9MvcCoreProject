using Cryptography.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace V9MvcCoreProject.DataDbContext;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Load configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Get your connection string (decrypt if needed)
        var connectionString = config.GetConnectionString("DefaultConnection");

        // If you're using your AES encryption helper:
        var crypt = new AesGcmEncryption(config);
        connectionString = crypt.Decrypt(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        // Pass isDesignTime=true to skip runtime DB creation logic
        return new ApplicationDbContext(optionsBuilder.Options, isDesignTime: true);
    }
}