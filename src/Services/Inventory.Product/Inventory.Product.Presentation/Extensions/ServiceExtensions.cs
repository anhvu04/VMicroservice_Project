using Infrastructure.ConfigurationService;
using Inventory.Product.Application.Extensions;
using Inventory.Product.Infrastructure.Extensions;
using Inventory.Product.Persistence.Extensions;
using Inventory.Product.Presentation.GrpcServerServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Inventory.Product.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.ConfigureGrpcClients();
        builder.ConfigureServices();
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

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
    }
}