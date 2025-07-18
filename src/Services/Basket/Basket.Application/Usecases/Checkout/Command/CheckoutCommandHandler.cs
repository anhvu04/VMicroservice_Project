using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Query.GetCart;
using Basket.Domain.DomainErrors;
using Basket.Domain.GenericRepository;
using Contracts.Common.Interfaces.MediatR;
using Contracts.Services.MessageBusService;
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
    private readonly ILogger<CheckoutCommandHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IMessageBusService _messageBusService;
    private readonly CartUtils _cartUtils;
    private const string BasketCheckoutRoutingKey = "basket-checkout-routing-key";

    public CheckoutCommandHandler(ILogger<CheckoutCommandHandler> logger,
        IMapper mapper, IBasketRepository basketRepository, CartUtils cartUtils, IMessageBusService messageBusService)
    {
        _logger = logger;
        _mapper = mapper;
        _basketRepository = basketRepository;
        _cartUtils = cartUtils;
        _messageBusService = messageBusService;
    }

    public async Task<Result> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartUtils.GetCartAsync(request.UserId);
        if (cart == null)
        {
            return Result.Failure("Cart is empty");
        }

        var enrichedCart = await _cartUtils.EnrichCheckoutCartAsync(cart);
        if (!enrichedCart.IsSuccess)
        {
            return Result.Failure(enrichedCart.Error!);
        }

        var checkoutEvent = _mapper.Map<BasketCheckoutEvent>(request);
        checkoutEvent.TotalPrice = enrichedCart.Value!.TotalPrice;
        enrichedCart.Value.CartItems.ForEach(x => checkoutEvent.Items.Add(_mapper.Map<BasketCheckoutEventItem>(x)));

        // publish event to rabbitmq
        await _messageBusService.PublishMessageAsync(checkoutEvent, BasketCheckoutRoutingKey, cancellationToken);
        _logger.LogInformation("Published basket checkout event for user {UserId}", request.UserId);

        // remove cart
        var cartKey = Utils.GetCartKey(request.UserId);
        await _basketRepository.DeleteDataAsync(cartKey);
        _logger.LogInformation("Removed cart for user {UserId}", request.UserId);

        return Result.Success();
    }
}