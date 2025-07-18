using Infrastructure.ConfigurationService;
using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.Infrastructure.RabbitMqService.IntegrationEventHandlers;
using Shared.ConfigurationSettings;

namespace Ordering.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransitMessageBus(configuration, ConfigureConsumers, ConfigureMessageBus);
    }

    private static void ConfigureConsumers(IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<BasketCheckoutEventHandler>();
    }

    private static void ConfigureMessageBus(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
        ConfigureBasketCheckoutQueue(cfg, ctx);
    }

    private static void ConfigureBasketCheckoutQueue(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
        cfg.ReceiveEndpoint("basket-checkout-queue",
            c =>
            {
                // Set ở consumer nghĩa là exchange type của queue = direct (masstransit sẽ tự động tạo 1 exchange theo tên của queue map 1-1). 
                // Tuy nhiên nếu exchange type = direct thì phải bind thêm routing key của exchange tự tạo với queue
                // c.ExchangeType = "direct";

                // Set false nghĩa là không tự động tạo exchange theo namespace của event
                c.ConfigureConsumeTopology = false;

                c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                c.Bind("basket-checkout-exchange",
                    x =>
                    {
                        // Set ở consumer nghĩa là routing key = binding key
                        x.RoutingKey = "basket-checkout-routing-key";
                        x.ExchangeType = "direct";
                    });
            });
    }
}