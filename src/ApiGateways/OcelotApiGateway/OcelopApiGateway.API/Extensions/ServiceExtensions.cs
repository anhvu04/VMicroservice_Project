using Infrastructure.ConfigurationService;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;

namespace OcelotApiGateway.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureOcelot();
        builder.ConfigureCors();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSwaggerOcelot();
    }

    private static void ConfigureOcelot(this WebApplicationBuilder builder)
    {
        builder.Services.AddOcelot().AddPolly().AddCacheManager(x => x.WithDictionaryHandle());
    }

    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    private static void ConfigureSwaggerOcelot(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerForOcelot(builder.Configuration, x =>
        {
            x.GenerateDocsForGatewayItSelf = true;
        });
    }
}