using EventBus.Messages.IntegrationEvent.Event;
using Identity.Presentation.Grpc.Protos;
using Infrastructure.ConfigurationService;
using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Abstractions;
using Notification.Application.Abstractions.EmailService;
using Notification.Infrastructure.EmailService;
using Notification.Infrastructure.Grpc.Clients;
using Notification.Infrastructure.RabbitMqService.IntegrationEventHandlers;
using Product.Presentation.Grpc.Protos;
using Shared.ConfigurationSettings;

namespace Notification.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureMassTransitMessageBus(configuration, ConfigureConsumers, ConfigureMessageBus);
        services.ConfigureGrpcClient(configuration);
        services.ConfigureEmailService(configuration);
    }

    private static void ConfigureConsumers(IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<CartNotificationScheduleEventHandler>();
    }

    private static void ConfigureMessageBus(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        ConfigureCartNotificationScheduleQueue(cfg, context);
    }

    private static void ConfigureCartNotificationScheduleQueue(IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint("cart-notification-schedule-queue", c =>
        {
            c.ConfigureConsumeTopology = false;
            c.ConfigureConsumer<CartNotificationScheduleEventHandler>(context);
            c.Bind("cart-notification-schedule-exchange", x =>
            {
                x.RoutingKey = "cart-notification-schedule-routing-key";
                x.ExchangeType = "direct";
            });
        });
    }

    private static void ConfigureGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        var grpcHostSettings = configuration.GetSection(nameof(GrpcHostSettings)).Get<GrpcHostSettings>() ??
                               throw new InvalidOperationException("GrpcHostSettings is not configured properly.");
        services.AddGrpcClient<CustomerInfoProtoService.CustomerInfoProtoServiceClient>(o =>
        {
            o.Address = new Uri(grpcHostSettings.CustomerUrl);
        });

        services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(o =>
        {
            o.Address = new Uri(grpcHostSettings.ProductUrl);
        });

        services.AddScoped<ICustomerSegmentService, CustomerInfoGrpcClient>();
        services.AddScoped<ICatalogProductService, ListProductsGrpcClient>();
    }

    private static void ConfigureEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)) ??
                                          throw new Exception("EmailSettings is not configured properly"));
        services.AddSingleton<ISmtpEmailService, SmtpEmailService>();
    }
}