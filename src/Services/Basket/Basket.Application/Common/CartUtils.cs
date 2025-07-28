using Basket.Application.Abstractions;
using Basket.Application.Usecases.Cart.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.Entities;
using Basket.Domain.GenericRepository;
using Microsoft.Extensions.Logging;
using Shared.InfrastructureGrpcModels.CartNotification;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.Utils;

namespace Basket.Application.Common;

public class CartUtils
{
    private const string Cart = "cart";
    private readonly ICatalogProductService _catalogProductService;
    private readonly ICartRepository _cartRepository;
    private readonly ICartNotificationScheduleService _cartNotificationScheduleService;
    private readonly ILogger<CartUtils> _logger;

    public CartUtils(ICatalogProductService catalogProductService, ICartRepository cartRepository,
        ICartNotificationScheduleService cartNotificationScheduleService, ILogger<CartUtils> logger)
    {
        _catalogProductService = catalogProductService;
        _cartRepository = cartRepository;
        _cartNotificationScheduleService = cartNotificationScheduleService;
        _logger = logger;
    }

    public string GetCartKey(Guid userId)
    {
        return Cart + ":" + userId;
    }

    /// <summary>
    /// Assumes that the cart is valid (cart items are valid)
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    public async Task<Result<GetCartResponse?>> EnrichGetCartAsync(Cart cart)
    {
        var cartKey = GetCartKey(cart.UserId);
        var productIds = cart.Items.Select(x => x.ProductId).ToList();
        var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
            new GetListCatalogProductsByIdGrpcBaseRequest
            {
                Ids = productIds
            });

        if (!products.IsSuccess)
        {
            _logger.LogError("Failed to get products: " + products.Error);
            return Result.Failure<GetCartResponse?>(CartErrors.ErrorGettingProducts);
        }

        // If no products are found, return empty cart
        if (products.Value!.Count == 0)
        {
            return new GetCartResponse
            {
                UserId = cart.UserId,
                CartItems = []
            };
        }

        var responseItems = new List<Items>();
        var validCartItems = new List<CartItems>();
        var cartItemDict = cart.Items.ToDictionary(x => x.ProductId);

        foreach (var item in products.Value)
        {
            cartItemDict.TryGetValue(item.Id, out var cartItem); // O(1)
            var quantity = cartItem?.Quantity ?? 0;
            responseItems.Add(new Items
            {
                ProductId = item.Id,
                ProductName = item.Name,
                ProductOriginalPrice = item.OriginalPrice,
                ProductSalePrice = item.SalePrice,
                Thumbnail = item.Thumbnail,
                Quantity = quantity
            });

            validCartItems.Add(new CartItems
            {
                ProductId = item.Id,
                Quantity = quantity
            });
        }

        // Update cart in Redis with only valid items
        var updatedCart = new Cart
        {
            UserId = cart.UserId,
            Items = validCartItems
        };

        var result = await _cartRepository.SaveCartAsync(cartKey, updatedCart);
        if (!result.IsSuccess)
        {
            Console.WriteLine("Failed to update cart: " + result.Error);
            return Result.Failure<GetCartResponse?>(CartErrors.ErrorUpdatingCart);
        }

        return new GetCartResponse
        {
            UserId = cart.UserId,
            CartItems = responseItems
        };
    }

    public async Task<Result<GetCartResponse?>> EnrichCheckoutCartAsync(Cart cart)
    {
        var productIds = cart.Items.Select(x => x.ProductId).ToList();
        var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
            new GetListCatalogProductsByIdGrpcBaseRequest
            {
                Ids = productIds
            });

        if (!products.IsSuccess)
        {
            _logger.LogError("Failed to get products: " + products.Error);
            return Result.Failure<GetCartResponse?>(CartErrors.ErrorGettingProducts);
        }

        if (products.Value!.Count == 0 || products.Value.Count != productIds.Count)
        {
            return Result.Failure<GetCartResponse?>(CartErrors.CartIncludeInvalidItem);
        }

        var items = new List<Items>();
        var cartItemDict = cart.Items.ToDictionary(x => x.ProductId);
        foreach (var item in products.Value)
        {
            var isExistProduct = cartItemDict.ContainsKey(item.Id);
            if (!isExistProduct)
            {
                return Result.Failure<GetCartResponse?>(CartErrors.CartIncludeInvalidItem);
            }

            items.Add(new Items
            {
                ProductId = item.Id,
                ProductName = item.Name,
                ProductOriginalPrice = item.OriginalPrice,
                ProductSalePrice = item.SalePrice,
                Thumbnail = item.Thumbnail,
                Quantity = cartItemDict[item.Id].Quantity
            });
        }

        return new GetCartResponse
        {
            UserId = cart.UserId,
            CartItems = items
        };
    }

    public async Task<string?> ScheduledJobAsync(Cart cart)
    {
        var result = await _cartNotificationScheduleService.SendCartNotificationScheduleAsync(
            new SendCartNotificationScheduleGrpcBaseRequest
            {
                UserId = cart.UserId,
                Items = cart.Items.Select(x => new SendCartItemsNotificationScheduleGrpcBaseRequest
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList(),
                LastModifiedDate = cart.LastModifiedDate,
                JobId = cart.JobId ?? string.Empty
            });

        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to schedule cart notification: " + result.Error);
            return null;
        }

        return result.Value!.JobId;
    }
}