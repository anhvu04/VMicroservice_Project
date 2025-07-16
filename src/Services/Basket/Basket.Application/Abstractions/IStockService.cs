namespace Basket.Application.Abstractions;

public interface IStockService
{
    Task<int> GetStockAsync(string productId);
}