using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
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

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureProductDbContext();
        builder.AddDependencyInjection();
    }

    private static void ConfigureProductDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine("ConnectionString: " + connectionString);
        var connectionBuilder = new MySqlConnectionStringBuilder(connectionString!);
        builder.Services.AddDbContext<ProductContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionBuilder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.Repositories");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));
    }

    private static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        builder.Services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
        builder.Services.AddScoped<ICatalogProductService, CatalogProductService>();

        // Add Mapper
        builder.Services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterMapping();
    }
}