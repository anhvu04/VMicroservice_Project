using MassTransit;

namespace EventBus.Messages.IntegrationEvent;

[ExcludeFromTopology]
public interface IIntegrationBaseEvent
{
    public DateTime CreationDate { get; }
    public Guid IdempotencyId { get; }
}