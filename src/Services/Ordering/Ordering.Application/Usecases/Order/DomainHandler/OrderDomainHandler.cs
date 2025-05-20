using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.OrderAggregate.Events;

namespace Ordering.Application.Usecases.Order.DomainHandler;

public class OrderDomainHandler : INotificationHandler<OrderCreatedEvent>, INotificationHandler<OrderDeletedEvent>
{
    private readonly ILogger<OrderDomainHandler> _logger;

    public OrderDomainHandler(ILogger<OrderDomainHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order with Id {OrderId} is successfully created.", notification.OrderId);
        return Task.CompletedTask;
    }

    public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order with Id {OrderId} is successfully deleted.", notification.OrderId);
        return Task.CompletedTask;
    }
}