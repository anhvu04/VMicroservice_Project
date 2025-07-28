using Ordering.Application.Usecases.Order.Common;
using Shared.MediatR;

namespace Ordering.Application.Usecases.Order.Query.GetOrder;

public record GetOrderQuery(Guid Id) : IQuery<OrderModel>;