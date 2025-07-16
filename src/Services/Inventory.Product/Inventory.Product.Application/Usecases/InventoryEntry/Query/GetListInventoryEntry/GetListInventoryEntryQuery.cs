using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Shared.Utils;
using Shared.Utils.Params;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetListInventoryEntry;

public class GetListInventoryEntryQuery : BaseQuery, IQuery<PaginationResult<GetInventoryEntryResponse>>
{
    public Guid ProductId { get; set; }
}