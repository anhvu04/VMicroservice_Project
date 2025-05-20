using Contracts.Domains.Interfaces;

namespace Contracts.Domains;

public abstract class EntityDateBase<TKey> : EntityBase<TKey>, IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}