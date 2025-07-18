using FluentValidation;

namespace Identity.Application.Usecases.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must have at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must have at least one non-alphanumeric character.");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.Password).WithMessage("Confirm password does not match.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"(84|0[3|5|7|8|9])+([0-9]{8})\b").WithMessage("Phone number is invalid.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
    }
}