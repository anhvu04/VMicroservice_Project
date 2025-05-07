using Contracts.Common.Interfaces.MediatR;
using Ordering.Application.Usecases.Order.Common;

namespace Ordering.Application.Usecases.Order.Query.GetOrder;

public record GetOrderQuery(Guid Id) : IQuery<OrderModel>;