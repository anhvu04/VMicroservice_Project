using Contracts.Services.MessageBusService;
using MassTransit;

namespace Infrastructure.Services.RabbitMqMessageBusService;

public class RabbitMqMessageBusService : IMessageBusService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMqMessageBusService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishMessageAsync<T>(T message, string routingKey,
        CancellationToken cancellationToken = default) where T : class
    {
        await _publishEndpoint.Publish(message, x => x.SetRoutingKey(routingKey), cancellationToken);
    }
}