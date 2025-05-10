using Contracts.Common.Interfaces.MediatR;
using EventBus.Messages.IntegrationEvent.Event;

namespace Ordering.Application.Usecases.Order.Command;

public record CreateOrderRequest : BasketCheckoutEvent, ICommand
{
}