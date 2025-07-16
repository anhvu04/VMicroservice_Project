namespace Inventory.Product.Application.Usecases.InventoryEntry.Common;

public class GetInventoryEntryResponse
{
    public string Id { get; set; } = null!;
    public string DocumentNo { get; set; } = null!;
    public string DocumentType { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    public int Quantity { get; set; }
    public string ExternalDocumentNo { get; set; } = null!;
}