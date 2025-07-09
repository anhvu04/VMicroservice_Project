using Contracts.Services.EmailService;
using Infrastructure.ConfigurationService;
using Infrastructure.Services.EmailService;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using Ordering.Persistence.Extensions;
using Shared.ConfigurationSettings;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructures(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.ConfigureServices();
    }

    private static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureClaimsRequirement();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerAuth();
        builder.ConfigureEmailService();
    }
}