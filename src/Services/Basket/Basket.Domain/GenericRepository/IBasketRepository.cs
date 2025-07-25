namespace Basket.Domain.GenericRepository;

public interface IBasketRepository
{
    Task<string?> GetDataByKeyAsync(string key);
    Task<bool> SetDataAsync(string key, object value, TimeSpan expiry);
    Task<bool> DeleteDataAsync(string key);
}