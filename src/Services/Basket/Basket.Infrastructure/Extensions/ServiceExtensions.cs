using Basket.Application.Abstractions;
using Basket.Infrastructure.Grpc.Clients;
using EventBus.Messages.IntegrationEvent.Event;
using HangFire.Presentation.Grpc.Protos;
using Infrastructure.ConfigurationService;
using Inventory.Product.Presentation.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Presentation.Grpc.Protos;
using Shared.ConfigurationSettings;

namespace Basket.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransitMessageBus(configuration, null, ConfigureMessageBus);
        services.ConfigureGrpcClient(configuration);
    }

    private static void ConfigureMessageBus(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        ConfigureBasketCheckoutEvent(cfg);
    }

    private static void ConfigureBasketCheckoutEvent(IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<BasketCheckoutEvent>(x =>
        {
            // set exchange name
            x.SetEntityName("basket-checkout-exchange");
        });
        cfg.Publish<BasketCheckoutEvent>(x =>
        {
            // set exchange type
            x.ExchangeType = "direct";
        });
    }

    private static void ConfigureGrpcClient(this IServiceCollection service, IConfiguration configuration)
    {
        var grpcHostSettings = configuration.GetSection(nameof(GrpcHostSettings)).Get<GrpcHostSettings>() ??
                               throw new InvalidOperationException("GrpcHostSettings is not configured properly.");

        service.AddGrpcClient<InventoryEntryProtoService.InventoryEntryProtoServiceClient>(c =>
        {
            c.Address = new Uri(grpcHostSettings.InventoryUrl);
        });

        service.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(c =>
        {
            c.Address = new Uri(grpcHostSettings.ProductUrl);
        });

        service.AddGrpcClient<CartNotificationScheduleService.CartNotificationScheduleServiceClient>(c =>
        {
            c.Address = new Uri(grpcHostSettings.ScheduledJobUrl);
        });

        service.AddScoped<IInventoryEntryService, GetInventoryEntryGrpcClientService>();
        service.AddScoped<ICatalogProductService, GetListProductsGrpcClientService>();
        service.AddScoped<ICartNotificationScheduleService, CartNotificationScheduleGrpcClientService>();
    }
}