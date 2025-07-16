using Basket.Application.Abstractions;
using Basket.Infrastructure.Grpc.Clients;
using EventBus.Messages.IntegrationEvent.Event;
using Inventory.Product.API.Protos;
using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.ConfigurationSettings;

namespace Basket.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransit(configuration);
       
        services.ConfigureGrpcClient(configuration);
    }

    private static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusSettings>(configuration.GetSection(nameof(EventBusSettings)));
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>()
                               ?? throw new InvalidOperationException("EventBusSettings is not configured properly.");

        services.TryAddSingleton(KebabCaseEndpointNameFormatter
            .Instance); // Ex: routing key: BasketCheckoutEvent -> basket-checkout-event

        Console.WriteLine("RabbitMQ Connection: " + eventBusSettings.Host + ":" + eventBusSettings.Port + ":" +
                          eventBusSettings.Username + ":" +
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

                cfg.Message<BasketCheckoutEvent>(x => { x.SetEntityName("basket-checkout-exchange"); });

                cfg.Publish<BasketCheckoutEvent>(x => { x.ExchangeType = ExchangeType.Direct.ToString(); });
            });
            // config.AddRequestClient<IBasketCheckoutEvent>();
        });
    }

   

    private static void ConfigureGrpcClient(this IServiceCollection service, IConfiguration configuration)
    {
        var grpcHostSettings = configuration.GetSection(nameof(GrpcHostSettings)).Get<GrpcHostSettings>() ??
                               throw new InvalidOperationException("GrpcHostSettings is not configured properly.");

        service.AddGrpcClient<StockProtoService.StockProtoServiceClient>(c =>
        {
            c.Address = new Uri(grpcHostSettings.InventoryUrl);
        });
        service.AddScoped<IStockService, GetStockGrpcClientService>();
    }
}