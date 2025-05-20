using Basket.Services.Models.Requests.Checkout;
using Basket.Services.Services.Interfaces;
using EventBus.Messages.IntegrationEvent.Event;
using EventBus.Messages.IntegrationEvent.Interface;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Basket.Services.Services.Implementation;

public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<CheckoutService> _logger;

    public CheckoutService(ICartService cartService, IMapper mapper, IPublishEndpoint publisher,
        ILogger<CheckoutService> logger)
    {
        _cartService = cartService;
        _mapper = mapper;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<Result> CheckoutAsync(CheckoutRequest request)
    {
        request.UserId = Guid.Parse("66d0125b-c4ed-4995-888c-7563044da14a");
        var cart = await _cartService.GetCartAsync(request.UserId);
        if (cart.IsFailure)
        {
            return Result.Failure(cart.Error ?? "Failed to get cart");
        }

        if (cart.Value!.CartItems.Count == 0)
        {
            return Result.Failure("Cart is empty");
        }

        var checkoutEvent = _mapper.Map<BasketCheckoutEvent>(request);
        checkoutEvent.TotalPrice = cart.Value.TotalPrice;
        cart.Value.CartItems.ForEach(x => checkoutEvent.Items.Add(_mapper.Map<BasketCheckoutEventItem>(x)));

        // publish event to rabbitmq
        await _publisher.Publish(checkoutEvent);
        _logger.LogInformation("Published basket checkout event for user {UserId}", request.UserId);

        // remove cart
        await _cartService.DeleteCartAsync(request.UserId);

        return Result.Success();
    }
}