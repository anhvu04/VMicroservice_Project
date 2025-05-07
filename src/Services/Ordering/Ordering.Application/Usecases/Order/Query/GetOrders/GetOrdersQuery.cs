using Contracts.Common.Interfaces.MediatR;
using Ordering.Application.Usecases.Order.Common;
using Shared.Utils.Pagination;

namespace Ordering.Application.Usecases.Order.Query.GetOrders;

public class GetOrdersQuery : PaginationParams, IQuery<PaginationResult<OrderModel>>
{
    public Guid? UserId { get; set; }
}