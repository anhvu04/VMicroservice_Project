using EventBus.Messages.IntegrationEvent.Event;
using Infrastructure.ConfigurationService;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ScheduledJob.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransitMessageBus(configuration, null, ConfigureMessageBus);
    }

    private static void ConfigureConsumers(IBusRegistrationConfigurator cfg)
    {
    }

    private static void ConfigureMessageBus(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
        ConfigureCartNotificationQueue(cfg, ctx);
    }

    private static void ConfigureCartNotificationQueue(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
        cfg.Message<CartNotificationScheduleEvent>(x =>
        {
            x.SetEntityName("cart-notification-schedule-exchange");
        });

        cfg.Publish<CartNotificationScheduleEvent>(x =>
        {
            x.ExchangeType = "direct";
        });
    }
}