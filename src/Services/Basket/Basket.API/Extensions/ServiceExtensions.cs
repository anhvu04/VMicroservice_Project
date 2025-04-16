using Basket.Repositories.Repositories.Implementation;
using Basket.Repositories.Repositories.Interfaces;
using Basket.Services.Services.Implementation;
using Basket.Services.Services.Interfaces;
using Basket.Services.Settings.Redis;
using StackExchange.Redis;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRedisDb(configuration);
        services.AddDependencyInjection();
    }

    private static void AddRedisDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var redisSettings = configuration.GetSection("ConnectionStrings:RedisConnection").Get<RedisSettings>()
                                ?? throw new InvalidOperationException("RedisConnection is not configured properly.");
            return ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints = { { redisSettings.EndPoints, redisSettings.Port } },
                User = redisSettings.User,
                Password = redisSettings.Password
            });
        });
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IBasketRepository, BasketRepository>();
        services.AddScoped<ICartService, CartService>();
    }
}