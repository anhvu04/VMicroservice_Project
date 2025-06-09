using Contracts.Domains;
using Infrastructure.Extensions.MongoDbExtensions;

namespace Inventory.Product.Repositories.Entities;

[BsonCollection("inventory_entries")]
public class InventoryEntry : MongoEntity
{
    public required string DocumentNo { get; set; }
    public DocumentType DocumentType { get; set; }
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
    public string? ExternalDocumentNo { get; set; }
}

public enum DocumentType
{
    All = 0,
    Purchase = 101,
    PurchaseInternal = 102,
    Sale = 201,
    SaleInternal = 202
}