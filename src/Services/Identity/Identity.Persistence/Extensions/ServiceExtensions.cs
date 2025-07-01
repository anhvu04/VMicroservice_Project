using Identity.Domain.UnitOfWork;
using Identity.Persistence.Persistence;
using Identity.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
    }
}