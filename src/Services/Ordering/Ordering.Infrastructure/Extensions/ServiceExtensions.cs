using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.Infrastructure.IntegrationEvents.EventHandlers;
using Shared.ConfigurationSettings;

namespace Ordering.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransit(configuration);
    }

    private static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
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
            config.AddConsumer<BasketCheckoutEventHandler>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(eventBusSettings.Host, (ushort)eventBusSettings.Port, "/", h =>
                {
                    h.Username(eventBusSettings.Username);
                    h.Password(eventBusSettings.Password);
                });

                cfg.ReceiveEndpoint("basket-checkout-queue",
                    c =>
                    {
                        c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                        c.Bind("basket-checkout-exchange",
                            x =>
                            {
                                x.RoutingKey = "basket-checkout-routing-key"; // Set ở consumer nghĩa là routing key = binding key
                                x.ExchangeType = ExchangeType.Direct.ToString();
                            });
                    });

                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}