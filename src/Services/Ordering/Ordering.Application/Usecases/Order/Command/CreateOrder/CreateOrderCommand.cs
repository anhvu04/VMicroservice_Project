using Contracts.Common.Interfaces.MediatR;
using EventBus.Messages.IntegrationEvent.Event;

namespace Ordering.Application.Usecases.Order.Command.CreateOrder;

public record CreateOrderCommand : BasketCheckoutEvent, ICommand
{
}