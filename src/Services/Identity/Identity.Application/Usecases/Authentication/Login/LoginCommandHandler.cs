using Contracts.Common.Interfaces.MediatR;
using Identity.Domain.Abstractions;
using Identity.Domain.DomainErrors;
using Identity.Domain.UnitOfWork;
using Shared.Utils;

namespace Identity.Application.Usecases.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtServices _jwtServices;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IIdentityUnitOfWork _identityUnitOfWork;

    public LoginCommandHandler(IJwtServices jwtServices, IIdentityUnitOfWork identityUnitOfWork,
        IPasswordHasher passwordHasher)
    {
        _jwtServices = jwtServices;
        _identityUnitOfWork = identityUnitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityUnitOfWork.Users.FindByConditionAsync(x => x.Email == request.Email,
            cancellationToken: cancellationToken);

        if (user == null)
        {
            return Result.Failure<LoginResponse>(UserErrors.UserNotFound);
        }

        if (!_passwordHasher.Verify(request.Password, user.Password))
        {
            return Result.Failure<LoginResponse>(UserErrors.WrongPassword);
        }

        var token = await _jwtServices.GenerateTokenAsync(user, cancellationToken);
        var refreshToken = await _jwtServices.GenerateRefreshTokenAsync();

        var response = new LoginResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FirstName + " " + user.LastName,
            Role = user.Role.ToString(),
            ImageUrl = user.ImageUrl,
            PhoneNumber = user.PhoneNumber,
            AccessToken = token,
            RefreshToken = refreshToken
        };

        return Result.Success(response);
    }
}