using Contracts.Common.Interfaces.MediatR;
using Ordering.Application.Usecases.Order.Common;
using Shared.Utils;
using Shared.Utils.Params;

namespace Ordering.Application.Usecases.Order.Query.GetOrders;

public class GetOrdersQuery : BaseQuery, IQuery<PaginationResult<OrderModel>>
{
    public Guid? UserId { get; set; }
}