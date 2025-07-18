using Contracts.Domains.Entity;
using Customer.Domain.GenericRepository;
using Customer.Persistence.Persistence;
using Infrastructure.Common.Implementation;

namespace Customer.Persistence.GenericRepository
{
    public class CustomerGenericRepository<TEntity, TKey> : GenericRepository<TEntity, TKey, CustomerContext>,
        ICustomerGenericRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        public CustomerGenericRepository(CustomerContext context) : base(context)
        {
        }
    }
}