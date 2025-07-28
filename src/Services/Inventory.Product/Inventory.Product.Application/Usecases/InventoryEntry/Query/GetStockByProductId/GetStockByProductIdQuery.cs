using Inventory.Product.Application.Usecases.InventoryEntry.Common;
using Shared.MediatR;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetStockByProductId;

public class GetStockByProductIdQuery(Guid productId) : IQuery<GetStockResponse>
{
    public Guid ProductId { get; set; } = productId;
}