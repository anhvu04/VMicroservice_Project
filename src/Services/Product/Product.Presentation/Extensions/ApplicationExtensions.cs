using Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using Product.Persistence.Persistence;
using Product.Presentation.Grpc.Protos;
using Product.Presentation.Grpc.Servers;

namespace Product.Presentation.Extensions;

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
        app.MapGrpcService<ProductGrpcServer>();
        app.MigrateDatabase<ProductContext>();
    }

    private static void MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<ProductContext>();
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