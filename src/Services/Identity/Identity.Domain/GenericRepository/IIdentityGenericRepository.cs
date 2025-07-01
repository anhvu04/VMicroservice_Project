using Contracts.Common.Interfaces;
using Contracts.Domains;

namespace Identity.Domain.GenericRepository;

public interface IIdentityGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
}