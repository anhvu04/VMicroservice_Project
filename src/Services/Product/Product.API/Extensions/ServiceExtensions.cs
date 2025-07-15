using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Infrastructure.ConfigurationService;
using Mapster.Utils;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.Repositories.Persistence;
using Product.Repositories.UnitOfWork;
using Product.Services.Mapping;
using Product.Services.Services.Implementation;
using Product.Services.Services.Interfaces;
using Shared.ConfigurationSettings;

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureProductDbContext();
        builder.ConfigureDependencyInjection();
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
    }

    private static void ConfigureProductDbContext(this WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>()
                               ?? throw new Exception("DatabaseSettings is not configured properly");
        Console.WriteLine("ConnectionString: " + databaseSettings.DefaultConnection);
        var connectionBuilder = new MySqlConnectionStringBuilder(databaseSettings.DefaultConnection);
        builder.Services.AddDbContext<ProductContext>(options =>
            options.UseMySql(databaseSettings.DefaultConnection,
                ServerVersion.AutoDetect(connectionBuilder.ConnectionString), e =>
                {
                    e.MigrationsAssembly("Product.Repositories");
                    e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                }));
    }

    private static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        builder.Services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
        builder.Services.AddScoped<ICatalogProductService, CatalogProductService>();

        // Add Mapper
        builder.Services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterMapping();
    }
}