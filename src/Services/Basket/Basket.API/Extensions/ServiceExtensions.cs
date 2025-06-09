using Basket.API.GrpcClientServices;
using Basket.Repositories.Repositories.Implementation;
using Basket.Repositories.Repositories.Interfaces;
using Basket.Services.Services.Implementation;
using Basket.Services.Services.Interfaces;
using Basket.Services.Settings.Redis;
using EventBus.Messages.IntegrationEvent.Event;
using Inventory.Product.API.Protos;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Shared.ConfigurationSettings;
using StackExchange.Redis;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddControllers();
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureRedisDb(configuration);
        builder.ConfigureMassTransit(configuration);
        builder.ConfigureDependencyInjection();
        builder.ConfigureGrpcClient();
    }

    private static void ConfigureGrpcClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(c =>
        {
            c.Address = new Uri(builder.Configuration["GrpcSettings:InventoryUrl"]!);
        });
        builder.Services.AddScoped<IStockService, GetStockGrpcClientService>();
    }

    private static void ConfigureRedisDb(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
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

    private static void ConfigureMassTransit(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.Services.Configure<EventBusSettings>(configuration.GetSection("EventBusSettings"));
        var eventBusSettings = configuration.GetSection("EventBusSettings").Get<EventBusSettings>()
                               ?? throw new InvalidOperationException("EventBusSettings is not configured properly.");

        builder.Services.TryAddSingleton(KebabCaseEndpointNameFormatter
            .Instance); // Ex: routing key: BasketCheckoutEvent -> basket-checkout-event

        Console.WriteLine(eventBusSettings.Host + ":" + eventBusSettings.Port + ":" + eventBusSettings.Username + ":" +
                          eventBusSettings.Password);

        builder.Services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(eventBusSettings.Host, (ushort)eventBusSettings.Port, "/", h =>
                {
                    h.Username(eventBusSettings.Username);
                    h.Password(eventBusSettings.Password);
                });

                cfg.Message<BasketCheckoutEvent>(x => { x.SetEntityName("basket-checkout-exchange"); });

                cfg.Publish<BasketCheckoutEvent>(x => { x.ExchangeType = ExchangeType.Direct; });
            });
            // config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }

    private static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBasketRepository, BasketRepository>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<ICheckoutService, CheckoutService>();
        builder.Services.AddScoped<IMapper, Mapper>();
    }
}