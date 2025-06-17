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
    private readonly IStockService _stockService;

    public CheckoutService(ICartService cartService, IMapper mapper, IPublishEndpoint publisher,
        ILogger<CheckoutService> logger, IStockService stockService)
    {
        _cartService = cartService;
        _mapper = mapper;
        _publisher = publisher;
        _logger = logger;
        _stockService = stockService;
    }

    public async Task<Result> CheckoutAsync(CheckoutRequest request)
    {
        request.UserId = Guid.Parse("66d0125b-c4ed-4995-888c-7563044da14a");
        var cart = await _cartService.GetCartAsync(request.UserId);
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
        await _publisher.Publish(checkoutEvent, x => x.SetRoutingKey("basket-checkout-routing-key"));
        _logger.LogInformation("Published basket checkout event for user {UserId}", request.UserId);

        // remove cart
        await _cartService.DeleteCartAsync(request.UserId);

        return Result.Success();
    }
}