using System.Text.Json;
using Basket.Domain.GenericRepository;
using StackExchange.Redis;

namespace Basket.Persistence.GenericRepository;

public class BasketRepository(IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<string?> GetDataByKeyAsync(string key)
    {
        var data = await _database.StringGetAsync(key);
        return data.IsNullOrEmpty ? null : data.ToString();
    }

    public async Task<bool> SetDataAsync(string key, object value, TimeSpan expiry)
    {
        var result =
            await _database.StringSetAsync(key, JsonSerializer.Serialize(value, _jsonSerializerOptions), expiry);
        return result;
    }

    public Task<bool> DeleteDataAsync(string key)
    {
        var result = _database.KeyDelete(key);
        return Task.FromResult(result);
    }
}