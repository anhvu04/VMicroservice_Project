using Contracts.Common.Interfaces;
using Contracts.Domains;
using Customer.Repositories.Persistence;

namespace Customer.Repositories.GenericRepository;

public interface ICustomerGenericRepository<TEntity, in TKey> : IGenericRepository<TEntity, TKey, CustomerContext>
    where TEntity : EntityBase<TKey>
{
}