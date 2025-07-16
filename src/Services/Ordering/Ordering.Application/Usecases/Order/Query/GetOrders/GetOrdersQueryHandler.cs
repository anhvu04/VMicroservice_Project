using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Usecases.Order.Common;
using Ordering.Domain.UnitOfWork;
using Shared.Utils;

namespace Ordering.Application.Usecases.Order.Query.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PaginationResult<OrderModel>>
{
    private readonly IOrderingUnitOfWork _orderingUnitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IMapper mapper, IOrderingUnitOfWork orderingUnitOfWork)
    {
        _mapper = mapper;
        _orderingUnitOfWork = orderingUnitOfWork;
    }

    public async Task<Result<PaginationResult<OrderModel>>> Handle(GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = _orderingUnitOfWork.Orders.FindAll(cancellationToken: cancellationToken);

        Expression<Func<Domain.Entities.Order, bool>> predicate = x => true;
        if (request.UserId.HasValue)
        {
            predicate = predicate.CombineAndAlsoExpressions(x => x.UserId == request.UserId);
        }

        orders = orders.Where(predicate);

        if (string.IsNullOrWhiteSpace(request.OrderBy))
        {
            orders = ApplySorting(orders, request);
        }

        var res = await orders.ProjectToPaginatedListAsync<Domain.Entities.Order, OrderModel>(request);
        return Result.Success(res);
    }


    #region Private Methods

    private static IQueryable<Domain.Entities.Order> ApplySorting(IQueryable<Domain.Entities.Order> orders,
        GetOrdersQuery request)
    {
        var orderBy = request.OrderBy!;
        var isDescending = request.IsDescending;

        return orderBy.ToLower().Replace(" ", "") switch
        {
            _ => orders.ApplySorting(isDescending, Domain.Entities.Order.GetSortValue(orderBy))
        };
    }

    #endregion
}