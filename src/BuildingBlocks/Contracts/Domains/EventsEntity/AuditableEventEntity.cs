using Contracts.Domains.Interfaces;

namespace Contracts.Domains.EventsEntity;

public class AuditableEventEntity<T> : EventEntity<T>, IEntityAuditBase<T>
{
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
}