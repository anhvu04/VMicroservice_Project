using Infrastructure.Common.Implementation;
using Ordering.Domain.UnitOfWork;
using Ordering.Persistence.Persistence;

namespace Ordering.Persistence.UnitOfWork;

public class OrderUnitOfWork<TContext> : UnitOfWork<TContext>, IOrderingUnitOfWork<TContext>
    where TContext : OrderingDbContext
{
    public OrderUnitOfWork(TContext context) : base(context)
    {
    }
}