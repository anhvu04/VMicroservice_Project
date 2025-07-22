using Shared.MediatR;

namespace Ordering.Application.Usecases.Order.Command.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) : ICommand
{
}