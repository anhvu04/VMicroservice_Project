using Contracts.Common.Events;

namespace Ordering.Domain.OrderAggregate.Events;

public class OrderDeletedEvent : BaseEvent
{
    public Guid OrderId { get; set; }

    public OrderDeletedEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}