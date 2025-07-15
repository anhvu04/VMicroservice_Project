using Basket.Domain.GenericRepository;
using Basket.Persistence.GenericRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.ConfigurationSettings;
using StackExchange.Redis;

namespace Basket.Persistence.Extensions;

public static class ServiceExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRedisDb(configuration);
        services.ConfigureDependencyInjection();
    }

    private static void ConfigureRedisDb(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>() ??
                               throw new InvalidOperationException("DatabaseSettings is not configured properly.");

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            // var redisSettings = configuration
            //                         .GetSection($"{nameof(DatabaseSettings)}:{nameof(DatabaseSettings.DefaultConnection)}")
            //                         .Get<RedisSettings>() ??
            //                     throw new InvalidOperationException("DatabaseSettings is not configured properly.");
            //
            //
            // return ConnectionMultiplexer.Connect(new ConfigurationOptions()
            // {
            //     EndPoints = { { redisSettings.EndPoints, redisSettings.Port } },
            //     User = redisSettings.User,
            //     Password = redisSettings.Password
            // });

            Console.WriteLine("Redis Connection: " + databaseSettings.DefaultConnection);

            return ConnectionMultiplexer.Connect(databaseSettings.DefaultConnection);
        });
    }

    private static void ConfigureDependencyInjection(this IServiceCollection service)
    {
        service.AddSingleton<IBasketRepository, BasketRepository>();
    }
}