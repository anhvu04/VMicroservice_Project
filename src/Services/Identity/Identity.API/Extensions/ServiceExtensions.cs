using Identity.Application.Extensions;
using Identity.Infrastructure.Extensions;
using Identity.Persistence.Extensions;
using Infrastructure.ConfigurationService;

namespace Identity.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication();
        builder.Services.AddPersistence(configuration);
        builder.Services.AddInfrastructure(configuration);
        builder.ConfigureServices();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
    }
}