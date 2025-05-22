using Inventory.Product.Services.Shared.Pagination;

namespace Inventory.Product.Services.Models.Requests.InventoryEntry;

public class GetInventoryEntryRequest : PaginationParams
{
    public Guid ProductId { get; set; }
}