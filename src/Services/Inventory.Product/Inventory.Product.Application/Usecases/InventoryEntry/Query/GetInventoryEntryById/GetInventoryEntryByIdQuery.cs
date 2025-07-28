using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Shared.MediatR;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetInventoryEntryById;

public class GetInventoryEntryByIdQuery(string id) : IQuery<GetInventoryEntryResponse>
{
    public string Id { get; set; } = id;
}