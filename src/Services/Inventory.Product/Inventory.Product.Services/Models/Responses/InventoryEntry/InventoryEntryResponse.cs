namespace Inventory.Product.Services.Models.Responses.InventoryEntry;

public class InventoryEntryResponse
{
    public string? Id { get; set; }
    public string? DocumentNo { get; set; }
    public string? DocumentType { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string? ExternalDocumentNo { get; set; }
}