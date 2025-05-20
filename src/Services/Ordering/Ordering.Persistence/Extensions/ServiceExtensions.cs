using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.UnitOfWork;
using Ordering.Persistence.Persistence;
using Ordering.Persistence.UnitOfWork;

namespace Ordering.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOrderingDbContext(configuration);
        services.AddDependencyInjection();
    }

    private static void ConfigureOrderingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<OrderingContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString);
        });
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IOrderingUnitOfWork, OrderingUnitOfWork>();
    }
}