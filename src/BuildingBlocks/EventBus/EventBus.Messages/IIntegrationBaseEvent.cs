namespace EventBus.Messages;

public interface IIntegrationBaseEvent
{
    public DateTime CreationDate { get; }
    public Guid Id { get; }
}