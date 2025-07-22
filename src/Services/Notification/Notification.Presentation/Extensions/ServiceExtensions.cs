using Infrastructure.ConfigurationService;
using Notification.Application.Extensions;
using Notification.Infrastructure.Extensions;
using Notification.Persistence.Extensions;

namespace Notification.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.ConfigureServices();
    }
    
    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // builder.ConfigureClaimsRequirement();
        // builder.ConfigureJwtAuthentication();
        // builder.ConfigureSwaggerAuth();
    }
}