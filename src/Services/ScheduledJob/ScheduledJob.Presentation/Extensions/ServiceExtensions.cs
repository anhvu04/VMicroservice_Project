using Infrastructure.ConfigurationService;
using ScheduledJob.Application.Extensions;
using ScheduledJob.Infrastructure.Extensions;
using ScheduledJob.Persistence.Extensions;

namespace ScheduledJob.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.ConfigureHangFire(configuration);
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