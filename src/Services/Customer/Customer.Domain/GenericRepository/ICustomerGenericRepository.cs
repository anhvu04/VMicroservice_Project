using Contracts.Common.Interfaces;
using Contracts.Domains.Entity;

namespace Customer.Domain.GenericRepository;

public interface ICustomerGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
}