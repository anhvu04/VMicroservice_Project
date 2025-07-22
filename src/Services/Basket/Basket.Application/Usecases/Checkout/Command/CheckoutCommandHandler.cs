using Basket.Application.Abstractions;
using Basket.Application.Common;
using Basket.Application.Usecases.Cart.Query.GetCart;
using Basket.Domain.DomainErrors;
using Basket.Domain.GenericRepository;
using Contracts.Services.MessageBusService;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.MediatR;
using Shared.Utils;

namespace Basket.Application.Usecases.Checkout.Command;

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand>
{
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutCommandHandler> _logger;
    private readonly ICartRepository _cartRepository;
    private readonly IMessageBusService _messageBusService;
    private readonly CartUtils _cartUtils;
    private const string BasketCheckoutRoutingKey = "basket-checkout-routing-key";

    public CheckoutCommandHandler(ILogger<CheckoutCommandHandler> logger,
        IMapper mapper, ICartRepository cartRepository, CartUtils cartUtils, IMessageBusService messageBusService)
    {
        _logger = logger;
        _mapper = mapper;
        _cartRepository = cartRepository;
        _cartUtils = cartUtils;
        _messageBusService = messageBusService;
    }

    public async Task<Result> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var cartKey = _cartUtils.GetCartKey(request.UserId);
        var cart = await _cartRepository.GetCartAsync(cartKey);
        if (!cart.IsSuccess)
        {
            return Result.Failure(cart.Error!);
        }

        var enrichedCart = await _cartUtils.EnrichCheckoutCartAsync(cart.Value!);
        if (!enrichedCart.IsSuccess)
        {
            return Result.Failure(enrichedCart.Error!);
        }

        var checkoutEvent = _mapper.Map<BasketCheckoutEvent>(request);
        checkoutEvent.TotalPrice = enrichedCart.Value!.TotalPrice;
        enrichedCart.Value.CartItems.ForEach(x => checkoutEvent.Items.Add(_mapper.Map<BasketCheckoutEventItem>(x)));

        // remove cart
        var deleteCartResult = await _cartRepository.DeleteCartAsync(cartKey);
        _logger.LogInformation("Removed cart for user {UserId}", request.UserId);
        if (!deleteCartResult.IsSuccess)
        {
            _logger.LogError("Failed to remove cart for user {UserId}", request.UserId);
            return Result.Failure("Failed to remove cart. Checkout failed. Please try again.");
        }

        // publish event to rabbitmq
        await _messageBusService.PublishMessageAsync(checkoutEvent, BasketCheckoutRoutingKey, cancellationToken);
        _logger.LogInformation("Published basket checkout event for user {UserId}", request.UserId);

        return Result.Success();
    }
}