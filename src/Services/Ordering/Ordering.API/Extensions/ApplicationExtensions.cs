using Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using Ordering.Persistence.Persistence;

namespace Ordering.API.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.MapControllers();
        app.UseAuthorization();
        app.MigrateDatabase<OrderingContext>();
    }

    private static void MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<OrderingContext>();
        try
        {
            logger.LogInformation("Start migrating database associated with context {DbContextName}",
                typeof(TContext).Name);
            ExecuteMigration(context);
            logger.LogInformation("End migrating database associated with context {DbContextName}",
                typeof(TContext).Name);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while migrating the database");
            throw;
        }
    }

    private static void ExecuteMigration<TContext>(TContext context) where TContext : DbContext
    {
        context.Database.Migrate();
    }
}