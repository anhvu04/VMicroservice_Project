using Infrastructure.Common.Implementation;
using Ordering.Domain.Entities;
using Ordering.Domain.GenericRepository;
using Ordering.Domain.UnitOfWork;
using Ordering.Persistence.GenericRepository;
using Ordering.Persistence.Persistence;

namespace Ordering.Persistence.UnitOfWork;

public class OrderingUnitOfWork : UnitOfWork<OrderingContext>,
    IOrderingUnitOfWork
{
    private readonly OrderingContext _context;

    public OrderingUnitOfWork(OrderingContext context) : base(context)
    {
        _context = context;
    }

    private IOrderingGenericRepository<Order, Guid>? _orders;
    private IOrderingGenericRepository<OrderDetail, Guid>? _orderDetails;

    public IOrderingGenericRepository<Order, Guid> Orders =>
        _orders ??= new OrderingGenericRepository<Order, Guid>(_context);

    public IOrderingGenericRepository<OrderDetail, Guid> OrderDetails =>
        _orderDetails ??= new OrderingGenericRepository<OrderDetail, Guid>(_context);
}