using Inventory.Product.Repositories.Abstraction;
using Inventory.Product.Repositories.Repository;
using Inventory.Product.Repositories.Settings;
using Inventory.Product.Services.Services.Implementation;
using Inventory.Product.Services.Services.Interfaces;
using MapsterMapper;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureInventoryDbClient(configuration);
        services.AddDependencyInjection();
    }

    private static void ConfigureInventoryDbClient(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>() ??
                              throw new Exception("MongoDbSettings is not configured");
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        string encodedPassword = Uri.EscapeDataString(mongoDbSettings.Password);

        var connectionString =
            $"mongodb://{mongoDbSettings.Username}:{encodedPassword}@{mongoDbSettings.Host}:{mongoDbSettings.Port}/{mongoDbSettings.DatabaseName}?authSource=admin";
        Console.WriteLine($"ConnectionString: {connectionString}");

        services.AddSingleton<IMongoClient>(new MongoClient(connectionString))
            .AddScoped(x => x.GetService<IMongoClient>()!.StartSession());
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IInventoryEntryService, InventoryEntryService>();
        services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
        services.AddScoped<IMapper, Mapper>();
    }
}