using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Contracts.Domains;

namespace Product.Repositories.Entities;

public class CatalogProduct : EntityAuditBase<Guid>
{
    public required string Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    public int OriginalPrice { get; set; }
    public int SalePrice { get; set; }
    public string? Thumbnail { get; set; }

    public static Expression<Func<CatalogProduct, object>> GetSortValue(string sort)
    {
        return sort switch
        {
            "originalPrice" => x => x.OriginalPrice,
            "salePrice" => x => x.SalePrice,
            _ => x => x.Name
        };
    }
}