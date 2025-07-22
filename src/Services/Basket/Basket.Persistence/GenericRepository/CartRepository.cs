using System.Text.Json;
using Basket.Domain.Entities;
using Basket.Domain.GenericRepository;
using Contracts.Services.CacheService;
using Shared.Utils;
using StackExchange.Redis;

namespace Basket.Persistence.GenericRepository;

public class CartRepository : ICartRepository
{
    private readonly ICacheService _cacheService;

    public CartRepository(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Result<Cart?>> GetCartAsync(string cartKey)
    {
        var cartDeserialize = await _cacheService.GetDataByKeyAsync(cartKey);
        if (string.IsNullOrEmpty(cartDeserialize))
        {
            return Result.Failure<Cart?>("Cart is empty or not found");
        }

        var cart = JsonSerializer.Deserialize<Cart>(cartDeserialize);
        if (cart == null || cart.Items.Count == 0)
        {
            return Result.Failure<Cart?>("Cart is empty or not found");
        }

        return cart;
    }

    public async Task<Result> SaveCartAsync(string cartKey, Cart cart)
    {
        var res = await _cacheService.SetDataAsync(cartKey, cart, TimeSpan.FromDays(180));
        return res ? Result.Success() : Result.Failure("Failed to save cart");
    }

    public async Task<Result> DeleteCartAsync(string cartKey)
    {
        var res = await _cacheService.DeleteDataAsync(cartKey);
        return res ? Result.Success() : Result.Failure("Failed to delete cart");
    }
}