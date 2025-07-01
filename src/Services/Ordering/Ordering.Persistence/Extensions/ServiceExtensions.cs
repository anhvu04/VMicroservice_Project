using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.UnitOfWork;
using Ordering.Persistence.Persistence;
using Ordering.Persistence.UnitOfWork;

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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        service.AddScoped<IOrderingUnitOfWork, OrderingUnitOfWork>();
    }
}