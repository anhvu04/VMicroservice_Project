namespace Inventory.Product.Services.Models.Requests.InventoryEntry;

public class PurchaseProductRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}