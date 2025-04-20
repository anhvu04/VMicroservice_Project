using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Persistence.Persistence;

namespace Ordering.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOrderingDbContext(configuration);
    }

    private static void ConfigureOrderingDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<OrderingDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString);
        });
    }
}