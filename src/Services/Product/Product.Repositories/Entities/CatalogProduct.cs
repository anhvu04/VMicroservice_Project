using System.ComponentModel.DataAnnotations;
using Contracts.Domains;

namespace Product.Repositories.Entities;

public class CatalogProduct : EntityAuditBase<Guid>
{
    public required string Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public string? Thumbnail { get; set; }
}