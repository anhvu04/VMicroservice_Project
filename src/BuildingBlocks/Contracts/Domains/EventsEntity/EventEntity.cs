using Contracts.Common.Interfaces.MediatR;
using Contracts.Domains.Entity;

namespace Contracts.Domains.EventsEntity;

public class EventEntity<T> : EntityBase<T>, IEventEntity<T>
{
    private readonly List<BaseEvent> _domainEvents = new();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public IReadOnlyCollection<BaseEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
}