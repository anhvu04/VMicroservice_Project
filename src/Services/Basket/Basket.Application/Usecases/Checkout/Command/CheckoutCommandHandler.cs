using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Query.GetCart;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Basket.Application.Usecases.Checkout.Command;

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<CheckoutCommandHandler> _logger;
    private readonly IStockService _stockService;
    private readonly ISender _sender;
    private readonly IBasketRepository _basketRepository;


    public CheckoutCommandHandler(ILogger<CheckoutCommandHandler> logger, IPublishEndpoint publisher, IMapper mapper,
        IStockService stockService, ISender sender, IBasketRepository basketRepository)
    {
        _logger = logger;
        _publisher = publisher;
        _mapper = mapper;
        _stockService = stockService;
        _sender = sender;
        _basketRepository = basketRepository;
    }

    public async Task<Result> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var cart = await _sender.Send(new GetCartQuery(request.UserId), cancellationToken);
        if (!cart.IsSuccess)
        {
            return Result.Failure(cart.Error ?? "Failed to get cart");
        }

        if (cart.Value!.CartItems.Count == 0)
        {
            return Result.Failure("Cart is empty");
        }

        foreach (var item in cart.Value.CartItems)
        {
            var stock = await _stockService.GetStockAsync(item.ProductId.ToString());
            if (item.Quantity > stock)
            {
                return Result.Failure($"Not enough stock for product {item.ProductId}");
            }
        }

        var checkoutEvent = _mapper.Map<BasketCheckoutEvent>(request);
        checkoutEvent.TotalPrice = cart.Value.TotalPrice;
        cart.Value.CartItems.ForEach(x => checkoutEvent.Items.Add(_mapper.Map<BasketCheckoutEventItem>(x)));

        // publish event to rabbitmq
        await _publisher.Publish(checkoutEvent, x => x.SetRoutingKey("basket-checkout-routing-key"),
            cancellationToken: cancellationToken);
        _logger.LogInformation("Published basket checkout event for user {UserId}", request.UserId);

        // remove cart
        var cartKey = Utils.GetCartKey(request.UserId);
        await _basketRepository.DeleteDataAsync(cartKey);
        _logger.LogInformation("Removed cart for user {UserId}", request.UserId);

        return Result.Success();
    }
}