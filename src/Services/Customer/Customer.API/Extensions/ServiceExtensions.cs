using Contracts.Common.Interfaces;
using Customer.Repositories.Persistence;
using Customer.Repositories.UnitOfWork;
using Customer.Services.Mapping;
using Customer.Services.Services.Implementation;
using Customer.Services.Services.Interfaces;
using Infrastructure.Common.Implementation;
using Infrastructure.ConfigurationService;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureCustomerDbContext();
        builder.ConfigureDependencyInjection();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
        builder.ConfigureClaimsRequirement();
    }

    private static void ConfigureCustomerDbContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"ConnectionString: {connectionString}");
        builder.Services.AddDbContext<CustomerContext>(options =>
            options.UseNpgsql(connectionString, e => { e.MigrationsAssembly("Customer.Repositories"); }));
    }

    private static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        builder.Services.AddScoped<ICustomerUnitOfWork, CustomerUnitOfWork>();
        builder.Services.AddScoped<ICustomerSegmentService, CustomerSegmentService>();

        // Add Mapper
        builder.Services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterConfig();
    }
}