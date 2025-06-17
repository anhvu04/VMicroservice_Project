namespace Basket.Services.Services.Interfaces;

public interface IStockService
{
    Task<int> GetStockAsync(string productId);
}