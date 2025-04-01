using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.Repositories.Persistence;
using Product.Repositories.UnitOfWork;
using Product.Services.Services.Implementation;
using Product.Services.Services.Interfaces;

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureProductDbContext(configuration);
        services.AddDependencyInjection();
    }

    private static void ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var builder = new MySqlConnectionStringBuilder(connectionString!);
        services.AddDbContext<ProductContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.Repositories");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
        services.AddScoped<ICatalogProductService, CatalogProductService>();
    }
}