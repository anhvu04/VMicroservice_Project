using Contracts.Common.Interfaces;
using Contracts.Domains.Entity;

namespace Product.Domain.GenericRepository;

public interface IProductGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
}