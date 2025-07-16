using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Inventory.Product.Domain.GenericRepository;
using Inventory.Product.Persistence.GenericRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace Inventory.Product.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureInventoryDb(configuration);
        services.ConfigureDependencyInjection();
    }

    private static void ConfigureInventoryDb(this IServiceCollection service, IConfiguration configuration)
    {
        var databaseSettings = configuration
            .GetSection(nameof(DatabaseSettings))
            .Get<DatabaseSettings>() ?? throw new Exception("DatabaseSettings is not configured properly");
        service.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

        Console.WriteLine($"ConnectionString: {databaseSettings.DefaultConnection}");
        service.AddSingleton<IMongoClient>(new MongoClient(databaseSettings.DefaultConnection))
            .AddScoped(x => x.GetService<IMongoClient>()!.StartSession());
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
        service.AddScoped<IInventoryEntryRepository, InventoryEntryRepository>();
    }
}