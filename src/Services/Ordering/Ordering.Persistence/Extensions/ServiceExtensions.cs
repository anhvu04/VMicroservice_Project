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
    public static void AddPersistence(this WebApplicationBuilder builder)
    {
        builder.ConfigureOrderingDbContext();
        builder.ConfigureDependencyInjection();
    }

    private static void ConfigureOrderingDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContextPool<OrderingContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        builder.Services.AddScoped<IOrderingUnitOfWork, OrderingUnitOfWork>();
    }
}