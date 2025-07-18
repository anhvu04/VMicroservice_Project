using Contracts.Domains;
using Contracts.Domains.Entity;
using Infrastructure.Common.Implementation;
using Ordering.Domain.GenericRepository;
using Ordering.Persistence.Persistence;

namespace Ordering.Persistence.GenericRepository;

public class OrderingGenericRepository<TEntity, TKey> : GenericRepository<TEntity, TKey, OrderingContext>,
    IOrderingGenericRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    public OrderingGenericRepository(OrderingContext context) : base(context)
    {
    }
}