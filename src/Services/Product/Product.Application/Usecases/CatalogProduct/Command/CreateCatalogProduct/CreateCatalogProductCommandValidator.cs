using FluentValidation;

namespace Product.Application.Usecases.CatalogProduct.Command.CreateCatalogProduct;

public class CreateCatalogProductCommandValidator : AbstractValidator<CreateCatalogProductCommand>
{
    public CreateCatalogProductCommandValidator()
    {
        RuleFor(x => x.OriginalPrice).GreaterThan(0).WithMessage("Original price must be greater than 0");
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0).WithMessage("Sale price must be greater than or equal to 0");
        RuleFor(x => x.SalePrice).LessThan(x => x.OriginalPrice)
            .WithMessage("Sale price must be less than original price");
    }
}