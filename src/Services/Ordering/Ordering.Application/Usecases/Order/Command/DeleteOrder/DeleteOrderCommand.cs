using Contracts.Common.Interfaces.MediatR;

namespace Ordering.Application.Usecases.Order.Command.DeleteOrder;

public record DeleteOrderCommand(Guid OrderId) : ICommand
{
}