using Contracts.Domains.Interfaces;

namespace Contracts.Domains.Entity;

public abstract class EntityDateBase<TKey> : EntityBase<TKey>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}