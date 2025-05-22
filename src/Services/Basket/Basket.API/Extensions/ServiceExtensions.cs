using Basket.Repositories.Repositories.Implementation;
using Basket.Repositories.Repositories.Interfaces;
using Basket.Services.Services.Implementation;
using Basket.Services.Services.Interfaces;
using Basket.Services.Settings.Redis;
using EventBus.Messages.IntegrationEvent.Event;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Shared.ConfigurationSettings;
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
        services.AddMassTransit(configuration);
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

    private static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusSettings>(configuration.GetSection("EventBusSettings"));
        var eventBusSettings = configuration.GetSection("EventBusSettings").Get<EventBusSettings>()
                               ?? throw new InvalidOperationException("EventBusSettings is not configured properly.");

        services.TryAddSingleton(KebabCaseEndpointNameFormatter
            .Instance); // Ex: routing key: BasketCheckoutEvent -> basket-checkout-event

        Console.WriteLine(eventBusSettings.Host + ":" + eventBusSettings.Port + ":" + eventBusSettings.Username + ":" +
                          eventBusSettings.Password);

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(eventBusSettings.Host, (ushort)eventBusSettings.Port, "/", h =>
                {
                    h.Username(eventBusSettings.Username);
                    h.Password(eventBusSettings.Password);
                });

                cfg.Message<BasketCheckoutEvent>(x =>
                {
                    x.SetEntityName("basket-checkout-event");
                });

                cfg.Publish<BasketCheckoutEvent>(x =>
                {
                    x.ExchangeType = ExchangeType.Direct;
                });
            });
            // config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }

    private static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IBasketRepository, BasketRepository>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<IMapper, Mapper>();
    }
}