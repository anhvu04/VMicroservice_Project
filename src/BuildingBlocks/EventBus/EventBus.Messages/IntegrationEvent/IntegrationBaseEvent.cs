using MassTransit;

namespace EventBus.Messages.IntegrationEvent;

[ExcludeFromTopology]
public record IntegrationBaseEvent : IIntegrationBaseEvent
{
    public DateTime CreationDate { get; } = DateTime.UtcNow;
    public Guid IdempotencyId { get; set; } = Guid.NewGuid();
}