using Contracts.Domains;
using Infrastructure.Common.Implementation;
using Ordering.Domain.GenericRepository;
using Ordering.Persistence.Persistence;

namespace Ordering.Persistence.GenericRepository;

public class OrderingGenericRepository<TEntity, TKey, TContext> : GenericRepository<TEntity, TKey, TContext>,
    IOrderingGenericRepository<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey> where TContext : OrderingDbContext
{
    public OrderingGenericRepository(TContext context) : base(context)
    {
    }
}