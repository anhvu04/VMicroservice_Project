using FluentValidation;

namespace Basket.Application.Usecases.Checkout.Command;

public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.ShippingFee).Must(x => x >= 0).WithMessage("Shipping fee must be greater than or equal to 0");
        RuleFor(x => x.PaymentMethod).Must(x => x >= 1).WithMessage("Payment method must be greater than or equal to 1");
    }
}