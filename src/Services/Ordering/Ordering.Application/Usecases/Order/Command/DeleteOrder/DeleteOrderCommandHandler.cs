using Contracts.Common.Interfaces.MediatR;
using Ordering.Domain.UnitOfWork;
using Shared.Utils;

namespace Ordering.Application.Usecases.Order.Command.DeleteOrder;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    private readonly IOrderingUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IOrderingUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.FindByIdAsync(request.OrderId, true, cancellationToken);
        if (order == null)
        {
            return Result.Failure($"Order {request.OrderId} not found.");
        }

        _unitOfWork.Orders.Delete(order);
        order.DeletedOrder();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}