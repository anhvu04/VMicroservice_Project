using System.ComponentModel.DataAnnotations;
using Infrastructure.Utils;

namespace Product.Services.Models.Requests.CatalogProduct;

public class UpdateCatalogProductRequest : ValidatableObject
{
    public string? Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    public int? OriginalPrice { get; set; }
    public int? SalePrice { get; set; }

    public string? Thumbnail { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalePrice.HasValue && OriginalPrice.HasValue && SalePrice.Value > OriginalPrice)
        {
            yield return Helper.CreateValidationResult("Sale price must be less than original price");
        }
    }
}