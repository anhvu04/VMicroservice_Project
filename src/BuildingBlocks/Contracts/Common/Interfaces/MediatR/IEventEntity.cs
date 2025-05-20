using Contracts.Common.Events;
using Contracts.Domains.Interfaces;

namespace Contracts.Common.Interfaces.MediatR;

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