using System.ComponentModel.DataAnnotations;
using Shared.Utils;

namespace Product.Services.Models.Requests.CatalogProduct;

public class CreateCatalogProductRequest : ValidatableObject
{
    public required string Name { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Original price must be greater than 0")]
    public required int OriginalPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Sale price must be greater than 0")]
    public int SalePrice { get; set; }

    public string? Thumbnail { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalePrice > OriginalPrice)
        {
            yield return Helper.CreateValidationResult("Sale price must be less than original price");
        }
    }
}