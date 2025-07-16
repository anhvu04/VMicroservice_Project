using Contracts.Common.Interfaces.MediatR;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Command.PurchaseProduct;

public class PurchaseProductCommand : ICommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}