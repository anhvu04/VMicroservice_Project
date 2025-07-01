using Identity.Domain.Abstractions;
using Identity.Infrastructure.BcryptService;
using Identity.Infrastructure.JwtService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.ConfigurationSettings;

namespace Identity.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureJwtService(configuration);
        services.ConfigureBcryptService();
    }

    private static void ConfigureJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
            throw new Exception("JwtSettings is not configured properly");
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddScoped<IJwtServices, JwtServices>();
    }

    private static void ConfigureBcryptService(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher, BCryptServices>();
    }
}