using FluentValidation;

namespace Inventory.Product.Application.Usecases.InventoryEntry.Command.PurchaseProduct;

public class PurchaseProductCommandValidator : AbstractValidator<PurchaseProductCommand>
{
    public PurchaseProductCommandValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}