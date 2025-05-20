using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Ordering.Application.Usecases.Order.Common;
using Ordering.Domain.UnitOfWork;
using Shared.Utils;

namespace Ordering.Application.Usecases.Order.Query.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, OrderModel>
{
    private readonly IOrderingUnitOfWork _orderingUnitOfWork;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(IOrderingUnitOfWork orderingUnitOfWork, IMapper mapper)
    {
        _orderingUnitOfWork = orderingUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderModel>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderingUnitOfWork.Orders.FindByIdAsync(request.Id, false, cancellationToken);
        if (orders == null)
        {
            return Result.Failure<OrderModel>($"Order with id {request.Id} not found");
        }

        var ordersModel = _mapper.Map<OrderModel>(orders);

        return Result.Success(ordersModel);
    }
}