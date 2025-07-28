using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Shared.MediatR;
using Shared.Utils;
using Shared.Utils.Params;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetListInventoryEntries;

public class GetListInventoryEntriesQuery : BaseQuery, IQuery<PaginationResult<GetInventoryEntryResponse>>
{
    public Guid ProductId { get; set; }
}