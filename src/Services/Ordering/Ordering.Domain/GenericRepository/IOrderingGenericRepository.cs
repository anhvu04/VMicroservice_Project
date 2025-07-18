using Contracts.Common.Interfaces;
using Contracts.Domains.Entity;

namespace Ordering.Domain.GenericRepository;

public interface IOrderingGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
}