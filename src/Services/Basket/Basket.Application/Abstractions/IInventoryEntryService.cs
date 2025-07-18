namespace Basket.Application.Abstractions;

public interface IInventoryEntryService
{
    Task<int> GetStockAsync(string productId);
}