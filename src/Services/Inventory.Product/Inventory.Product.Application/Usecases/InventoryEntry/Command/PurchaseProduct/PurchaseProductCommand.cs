using Contracts.Common.Interfaces.MediatR;
using Inventory.Product.Application.Usecases.InventoryEntry.Common;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Command.PurchaseProduct;

public class PurchaseProductCommand : ICommand<PurchaseProductResponse>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}