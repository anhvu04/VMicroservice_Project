using EventBus.Messages.IntegrationEvent.Event;
using Shared.MediatR;

namespace Ordering.Application.Usecases.Order.Command.CreateOrder;

public record CreateOrderCommand : BasketCheckoutEvent, ICommand
{
}