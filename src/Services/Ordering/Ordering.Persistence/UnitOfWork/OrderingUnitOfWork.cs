using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using Infrastructure.Common.Implementation;
using Infrastructure.Extensions.MediatorExtentions;
using MediatR;
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
    private readonly IMediator _mediator;

    public OrderingUnitOfWork(OrderingContext context, IMediator mediator) : base(context)
    {
        _context = context;
        _mediator = mediator;
    }

    private IOrderingGenericRepository<Order, Guid>? _orders;
    private IOrderingGenericRepository<OrderDetail, Guid>? _orderDetails;

    public IOrderingGenericRepository<Order, Guid> Orders =>
        _orders ??= new OrderingGenericRepository<Order, Guid>(_context);

    public IOrderingGenericRepository<OrderDetail, Guid> OrderDetails =>
        _orderDetails ??= new OrderingGenericRepository<OrderDetail, Guid>(_context);

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var events = GetEventsBeforeSaveChanges();
        // nếu save change sau dispatch event, thì sẽ diễn ra không đồng bộ dữ liệu (event đã được gửi đi nhưng trong db lại chưa có data).
        // Vì vậy nên save change trước khi dispatch. Tuy nhiên vẫn còn trường hợp nếu event publish thất bại thì dữ liệu trong db sẽ bị rollback.
        var res = await base.SaveChangesAsync(cancellationToken);
        if (events.Count != 0)
        {
            await _mediator.DispatchDomainEventAsync(events);
        }

        return res;
    }

    private List<BaseEvent> GetEventsBeforeSaveChanges()
    {
        var domainEntities = _context.ChangeTracker.Entries<IEventEntity>()
            .Select(x => x.Entity)
            .Where(x => x.GetDomainEvents().Count != 0).ToList();

        var domainEvents = domainEntities.SelectMany(x => x.GetDomainEvents()).ToList();

        domainEntities.ForEach(x => x.ClearDomainEvents());
        return domainEvents;
    }
}