namespace Contracts.Services.MessageBusService;

public interface IMessageBusService
{
    Task PublishMessageAsync<T>(T message, string routingKey, CancellationToken cancellationToken = default) where T : class;
}