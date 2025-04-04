using Contracts.Common.Interfaces;
using Customer.Repositories.Persistence;
using Customer.Repositories.UnitOfWork;
using Customer.Services.Mapping;
using Customer.Services.Services.Implementation;
using Customer.Services.Services.Interfaces;
using Infrastructure.Common.Implementation;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureCustomerDbContext(configuration);
        services.AddDependencyInjection();
    }

    private static void ConfigureCustomerDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"ConnectionString: {connectionString}");
        services.AddDbContext<CustomerContext>(options =>
            options.UseNpgsql(connectionString, e => { e.MigrationsAssembly("Customer.Repositories"); }));
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<ICustomerUnitOfWork, CustomerUnitOfWork>();
        services.AddScoped<ICustomerSegmentService, CustomerSegmentService>();
        services.AddSingleton<IPasswordHashing, PasswordHashing>();

        // Add Mapper
        services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterConfig();
    }
}