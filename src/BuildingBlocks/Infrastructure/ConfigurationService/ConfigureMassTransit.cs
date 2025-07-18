using Contracts.Services.MessageBusService;
using Infrastructure.Services.RabbitMqMessageBusService;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.ConfigurationSettings;

namespace Infrastructure.ConfigurationService;

public static class ConfigureMassTransit
{
    public static void ConfigureMassTransitMessageBus(this IServiceCollection services, IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? registerConsumers = null,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureBus = null)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>()
                               ?? throw new InvalidOperationException("EventBusSettings is not configured properly.");
        services.Configure<EventBusSettings>(configuration.GetSection(nameof(EventBusSettings)));

        // Ex: routing key: BasketCheckoutEvent -> basket-checkout-event
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

        Console.WriteLine("RabbitMQ: " + eventBusSettings.Host + ":" + eventBusSettings.Port + ":" +
                          eventBusSettings.Username + ":" + eventBusSettings.Password);

        services.AddMassTransit(cfg =>
        {
            // Cho phép service gọi thêm cfg.AddConsumer<…>()
            registerConsumers?.Invoke(cfg);

            cfg.UsingRabbitMq((context, busCfg) =>
            {
                busCfg.Host(eventBusSettings.Host, (ushort)eventBusSettings.Port, "/", h =>
                {
                    h.Username(eventBusSettings.Username);
                    h.Password(eventBusSettings.Password);
                });

                // Cho phép service tuỳ chỉnh topology, receive‑endpoint, ...
                configureBus?.Invoke(busCfg, context);

                // Tự gắn tất cả consumers đã đăng ký
                busCfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessageBusService, RabbitMqMessageBusService>();
    }
}