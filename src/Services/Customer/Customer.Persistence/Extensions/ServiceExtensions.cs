using Contracts.Common.Interfaces;
using Customer.Domain.UnitOfWork;
using Customer.Persistence.Persistence;
using Customer.Persistence.UnitOfWork;
using Infrastructure.Common.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.ConfigurationSettings;

namespace Customer.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureCustomerDbContext(configuration);
        services.ConfigureDependencyInjection();
    }

    private static void ConfigureCustomerDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
            .Get<DatabaseSettings>() ?? throw new Exception("DatabaseSettings is not configured properly.");

        Console.WriteLine($"ConnectionString: {databaseSettings.DefaultConnection}");
        services.AddDbContext<CustomerContext>(options => options.UseNpgsql(databaseSettings.DefaultConnection));
    }

    private static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<ICustomerUnitOfWork, CustomerUnitOfWork>();
    }

    public static void MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<CustomerContext>();
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