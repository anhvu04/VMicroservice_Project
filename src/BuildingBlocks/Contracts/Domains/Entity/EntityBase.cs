using Contracts.Domains.Interfaces;

namespace Contracts.Domains.Entity;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}