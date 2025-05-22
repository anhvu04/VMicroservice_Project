using Inventory.Product.Services.Models.Requests.InventoryEntry;
using Inventory.Product.Services.Models.Responses.InventoryEntry;
using Inventory.Product.Services.Shared;
using Inventory.Product.Services.Shared.Pagination;


namespace Inventory.Product.Services.Services.Interfaces;

public interface IInventoryEntryService
{
    Task<Result<PaginationResult<InventoryEntryResponse>>> GetInventoryEntries(GetInventoryEntryRequest request);
    Task<Result<InventoryEntryResponse>> GetInventoryEntryById(string id);
    Task<Result> PurchaseProduct(PurchaseProductRequest request);
}