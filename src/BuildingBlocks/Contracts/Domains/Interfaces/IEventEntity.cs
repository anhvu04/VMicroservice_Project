using Contracts.Domains.EventsEntity;

namespace Contracts.Domains.Interfaces;

public interface IEventEntity
{
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvents();
    IReadOnlyCollection<BaseEvent> GetDomainEvents();
}

public interface IEventEntity<T> : IEntityBase<T>, IEventEntity
{
}