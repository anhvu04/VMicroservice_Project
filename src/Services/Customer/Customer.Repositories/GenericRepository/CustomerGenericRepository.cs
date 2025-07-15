using Contracts.Domains;
using Contracts.Domains.Entity;
using Customer.Repositories.Persistence;
using Infrastructure.Common.Implementation;

namespace Customer.Repositories.GenericRepository;

public class CustomerGenericRepository<TEntity, TKey> : GenericRepository<TEntity, TKey, CustomerContext>,
    ICustomerGenericRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    public CustomerGenericRepository(CustomerContext context) : base(context)
    {
    }
}