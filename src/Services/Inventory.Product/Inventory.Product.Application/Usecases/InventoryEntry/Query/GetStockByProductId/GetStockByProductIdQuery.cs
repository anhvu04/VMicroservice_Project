using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Query.GetStockByProductId;

public class GetStockByProductIdQuery(Guid productId) : IQuery<GetStockResponse>
{
    public Guid ProductId { get; set; } = productId;
}