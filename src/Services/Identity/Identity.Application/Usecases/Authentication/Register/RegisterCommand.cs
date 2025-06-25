using Contracts.Common.Interfaces.MediatR;

namespace Identity.Application.Usecases.Authentication.Register;

public class RegisterCommand : ICommand
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }

    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
}