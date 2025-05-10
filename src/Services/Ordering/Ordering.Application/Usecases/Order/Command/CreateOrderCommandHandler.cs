using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Ordering.Domain.Entities;
using Ordering.Domain.UnitOfWork;
using Shared.Utils;

namespace Ordering.Application.Usecases.Order.Command;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderRequest>
{
    private readonly IOrderingUnitOfWork _orderingUnitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderingUnitOfWork orderingUnitOfWork, IMapper mapper)
    {
        _orderingUnitOfWork = orderingUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Domain.Entities.Order
        {
            UserId = request.UserId,
            TotalPrice = request.TotalPrice,
            ShippingFee = request.ShippingFee,
            TotalAmount = request.TotalPrice + request.ShippingFee,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            OrderStatus = OrderStatus.Pending,
            PaymentMethod = (PaymentMethod)request.PaymentMethod,
        };

        _orderingUnitOfWork.Orders.Add(order);

        List<OrderDetail> orderDetails = [];
        request.Items.ForEach(item =>
        {
            orderDetails.Add(new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.ProductPrice,
                ItemPrice = item.ProductPrice * item.Quantity
            });
        });

        _orderingUnitOfWork.OrderDetails.AddRange(orderDetails);

        await _orderingUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}