using FluentValidation;

namespace Product.Application.Usecases.CatalogProduct.Command.UpdateCatalogProduct;

public class UpdateCatalogProductCommandValidator : AbstractValidator<UpdateCatalogProductCommand>
{
    public UpdateCatalogProductCommandValidator()
    {
        RuleFor(x => x.OriginalPrice)
            .GreaterThan(0).When(x => x.OriginalPrice.HasValue).WithMessage("Original price must be greater than 0");
        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0).When(x => x.SalePrice.HasValue)
            .WithMessage("Sale price must be greater than or equal to 0");
        RuleFor(x => x.SalePrice)
            .LessThanOrEqualTo(x => x.OriginalPrice).When(x => x.SalePrice.HasValue && x.OriginalPrice.HasValue)
            .WithMessage("Sale price must be less than or equal to original price");
    }
}