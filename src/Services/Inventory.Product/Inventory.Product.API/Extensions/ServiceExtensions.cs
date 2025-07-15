using Contracts.Common.Interfaces;
using Infrastructure.Common.Implementation;
using Infrastructure.ConfigurationService;
using Inventory.Product.API.GrpcServerServices;
using Inventory.Product.Services.Services.Implementation;
using Inventory.Product.Services.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using Shared.ConfigurationSettings;

namespace Inventory.Product.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.ConfigureInventoryDb();
        builder.ConfigureDependencyInjection();
        builder.ConfigureGrpcClients();
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
    }

    private static void ConfigureGrpcClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpc();

        builder.WebHost.ConfigureKestrel(options =>
        {
            var port = builder.Configuration.GetSection(nameof(GrpcPortSettings)).Get<GrpcPortSettings>() ??
                       throw new Exception("GrpcPortSettings is null");
            options.ListenAnyIP(port.InventoryHttp1, o => { o.Protocols = HttpProtocols.Http1; });
            options.ListenAnyIP(port.InventoryHttp2, o => { o.Protocols = HttpProtocols.Http2; });
        });
    }

    private static void ConfigureInventoryDb(this WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration
            .GetSection(nameof(DatabaseSettings))
            .Get<DatabaseSettings>() ?? throw new Exception("DatabaseSettings is not configured properly");
        builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));

        Console.WriteLine($"ConnectionString: {databaseSettings.DefaultConnection}");
        builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseSettings.DefaultConnection))
            .AddScoped(x => x.GetService<IMongoClient>()!.StartSession());
    }

    private static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IInventoryEntryService, InventoryEntryService>();
        builder.Services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
        builder.Services.AddScoped<IMapper, Mapper>();
    }
}