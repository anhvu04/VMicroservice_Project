using Identity.Domain.UnitOfWork;
using Identity.Persistence.Persistence;
using Identity.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.ConfigurationSettings;

namespace Identity.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection service, IConfiguration configuration)
    {
        service.ConfigureIdentityDbContext(configuration);
        service.ConfigureDependencyInjection();
    }

    private static void ConfigureIdentityDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContextPool<IdentityContext>(options =>
        {
            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>() ??
                                   throw new Exception("Database settings is not configured properly.");
            Console.WriteLine(databaseSettings.DefaultConnection);
            options.UseSqlServer(databaseSettings.DefaultConnection);
        });
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
    }
}