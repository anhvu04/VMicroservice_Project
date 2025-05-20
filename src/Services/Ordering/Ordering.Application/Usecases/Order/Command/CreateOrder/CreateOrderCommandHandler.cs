using Contracts.Common.Interfaces.MediatR;
using MapsterMapper;
using Ordering.Domain.Entities;
using Ordering.Domain.UnitOfWork;
using Shared.Utils;

namespace Ordering.Application.Usecases.Order.Command.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly IOrderingUnitOfWork _orderingUnitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderingUnitOfWork orderingUnitOfWork, IMapper mapper)
    {
        _orderingUnitOfWork = orderingUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Domain.Entities.Order
        {
            UserId = command.UserId,
            TotalPrice = command.TotalPrice,
            ShippingFee = command.ShippingFee,
            TotalAmount = command.TotalPrice + command.ShippingFee,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Address = command.Address,
            PhoneNumber = command.PhoneNumber,
            OrderStatus = OrderStatus.Pending,
            PaymentMethod = (PaymentMethod)command.PaymentMethod,
        };

        _orderingUnitOfWork.Orders.Add(order);

        List<OrderDetail> orderDetails = [];
        command.Items.ForEach(item =>
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
        order.AddedOrder();
        await _orderingUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}