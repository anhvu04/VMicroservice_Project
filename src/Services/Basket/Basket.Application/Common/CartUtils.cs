using System.Text.Json;
using Basket.Application.Abstractions;
using Basket.Application.Usecases.Cart.Common;
using Basket.Domain.DomainErrors;
using Basket.Domain.Entities;
using Basket.Domain.GenericRepository;
using Shared.InfrastructureServiceModels.GetListCatalogProductsByIdModel;
using Shared.Utils;

namespace Basket.Application.Common;

public class CartUtils
{
    private readonly ICatalogProductService _catalogProductService;
    private readonly IBasketRepository _basketRepository;

    public CartUtils(ICatalogProductService catalogProductService, IBasketRepository basketRepository)
    {
        _catalogProductService = catalogProductService;
        _basketRepository = basketRepository;
    }

    public async Task<Cart?> GetCartAsync(Guid userId)
    {
        var cartKey = Utils.GetCartKey(userId);
        var cartDeserialize = await _basketRepository.GetDataByKeyAsync(cartKey);
        if (string.IsNullOrEmpty(cartDeserialize))
        {
            return null;
        }

        var cart = JsonSerializer.Deserialize<Cart>(cartDeserialize);
        if (cart == null || cart.Items.Count == 0)
        {
            return null;
        }

        return new Cart
        {
            UserId = userId,
            Items = cart.Items
        };
    }

    /// <summary>
    /// Assumes that the cart is valid (cart items are valid)
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    public async Task<Result<GetCartResponse?>> EnrichGetCartAsync(Cart cart)
    {
        try
        {
            var cartKey = Utils.GetCartKey(cart.UserId);
            var productIds = cart.Items.Select(x => x.ProductId).ToList();
            var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
                new GetListCatalogProductsByIdRequest
                {
                    Ids = productIds
                });

            // If no products are found, return empty cart
            if (products.Count == 0)
            {
                var emptyCart = new Cart
                {
                    UserId = cart.UserId,
                    Items = []
                };

                // Update cart in Redis with empty cart
                await _basketRepository.SetDataAsync(cartKey, emptyCart, TimeSpan.FromDays(180));

                return new GetCartResponse
                {
                    UserId = cart.UserId,
                    CartItems = []
                };
            }

            var responseItems = new List<Items>();
            var validCartItems = new List<CartItems>();

            foreach (var item in products)
            {
                responseItems.Add(new Items
                {
                    ProductId = item.Id,
                    ProductName = item.Name,
                    ProductOriginalPrice = item.OriginalPrice,
                    ProductSalePrice = item.SalePrice,
                    Thumbnail = item.Thumbnail,
                    Quantity = cart.Items.FirstOrDefault(x => x.ProductId == item.Id)?.Quantity ?? 0
                });

                validCartItems.Add(new CartItems
                {
                    ProductId = item.Id,
                    Quantity = cart.Items.FirstOrDefault(x => x.ProductId == item.Id)?.Quantity ?? 0
                });
            }

            // Update cart in Redis with only valid items
            var updatedCart = new Cart
            {
                UserId = cart.UserId,
                Items = validCartItems
            };

            await _basketRepository.SetDataAsync(cartKey, updatedCart, TimeSpan.FromDays(180));

            return new GetCartResponse
            {
                UserId = cart.UserId,
                CartItems = responseItems
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to enrich cart: " + ex.Message);
            return Result.Failure<GetCartResponse?>(CartErrors.ErrorGettingCart);
        }
    }

    public async Task<Result<GetCartResponse?>> EnrichCheckoutCartAsync(Cart cart)
    {
        try
        {
            var productIds = cart.Items.Select(x => x.ProductId).ToList();
            var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
                new GetListCatalogProductsByIdRequest
                {
                    Ids = productIds
                });

            if (products.Count == 0 || products.Count != productIds.Count)
            {
                return Result.Failure<GetCartResponse?>("Cart includes items that are not available");
            }

            var items = new List<Items>();
            foreach (var item in cart.Items)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product == null)
                {
                    return Result.Failure<GetCartResponse?>("Cart includes items that are not available");
                }

                items.Add(new Items
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductOriginalPrice = product.OriginalPrice,
                    ProductSalePrice = product.SalePrice,
                    Thumbnail = product.Thumbnail,
                    Quantity = item.Quantity,
                });
            }

            return new GetCartResponse
            {
                UserId = cart.UserId,
                CartItems = items
            };
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to enrich cart: " + e.Message);
            return Result.Failure<GetCartResponse?>(CartErrors.ErrorGettingCart);
        }
    }
}