using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.UnitOfWork;
using Ordering.Persistence.Persistence;
using Ordering.Persistence.UnitOfWork;
using Shared.ConfigurationSettings;

namespace Ordering.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection service, IConfiguration configuration)
    {
        service.ConfigureOrderingDbContext(configuration);
        service.ConfigureDependencyInjection();
    }

    private static void ConfigureOrderingDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContextPool<OrderingContext>(options =>
        {
            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>() ??
                                   throw new Exception("DatabaseSettings is not configured properly");
            Console.WriteLine("Connection string: " + databaseSettings.DefaultConnection);
            options.UseSqlServer(databaseSettings.DefaultConnection);
        });
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        service.AddScoped<IOrderingUnitOfWork, OrderingUnitOfWork>();
    }
}