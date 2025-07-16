using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetInventoryEntryById;

public class GetInventoryEntryByIdQuery(string id) : IQuery<GetInventoryEntryResponse>
{
    public string Id { get; set; } = id;
}