using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.Domain.UnitOfWork;
using Product.Persistence.Persistence;
using Product.Persistence.UnitOfWork;
using Shared.ConfigurationSettings;

namespace Product.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureProductDbContext(configuration);
        services.ConfigureDependencyInjection();
    }

    private static void ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>()
                               ?? throw new Exception("DatabaseSettings is not configured properly");
        Console.WriteLine("ConnectionString: " + databaseSettings.DefaultConnection);
        var connectionBuilder = new MySqlConnectionStringBuilder(databaseSettings.DefaultConnection);
        services.AddDbContext<ProductContext>(options => options.UseMySql(databaseSettings.DefaultConnection,
            ServerVersion.AutoDetect(connectionBuilder.ConnectionString),
            e => { e.SchemaBehavior(MySqlSchemaBehavior.Ignore); }));
    }

    private static void ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
    }
}