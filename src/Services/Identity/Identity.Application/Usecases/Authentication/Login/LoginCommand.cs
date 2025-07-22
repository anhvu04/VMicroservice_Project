using System.ComponentModel.DataAnnotations;
using Shared.MediatR;

namespace Identity.Application.Usecases.Authentication.Login;

public class LoginCommand : ICommand<LoginResponse>
{
    [EmailAddress] public required string Email { get; set; }
    public required string Password { get; set; }
}