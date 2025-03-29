using Microsoft.EntityFrameworkCore;
using Product.API.Persistence;

namespace Product.API.Extensions;

public static class HostExtensions
{
    public static void MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<ProductContext>();
            try
            {
                logger.LogInformation("Migrating database...");
                ExecuteMigration(context);
                logger.LogInformation("Migrated database successfully!");
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while migrating the database");
                throw;
            }
        }
    }

    private static void ExecuteMigration<TContext>(TContext context) where TContext : DbContext
    {
        context.Database.Migrate();
    }
}