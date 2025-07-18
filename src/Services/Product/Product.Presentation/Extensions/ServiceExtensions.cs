using Infrastructure.ConfigurationService;
using Product.Application.Extensions;
using Product.Infrastructure.Extensions;
using Product.Persistence.Extensions;

namespace Product.Presentation.Extensions;

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
        builder.ConfigureGrpcServers();
        builder.ConfigureServices();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
    }
}