using Contracts.Common.Interfaces.MediatR;
using Identity.Domain.Abstractions;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.Domain.UnitOfWork;
using Shared.Enums;
using Shared.Utils;

namespace Identity.Application.Usecases.Authentication.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IIdentityUnitOfWork _identityUnitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IIdentityUnitOfWork identityUnitOfWork, IPasswordHasher passwordHasher)
    {
        _identityUnitOfWork = identityUnitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = await _identityUnitOfWork.Users.FindAnyAsync(x => x.Email == request.Email,
            cancellationToken: cancellationToken);

        if (isUserExist)
        {
            return Result.Failure(UserErrors.UserAlreadyExists);
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = _passwordHasher.Hash(request.Password),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Role = UserRoles.Customer,
            IsEmailConfirmed = false
        };

        _identityUnitOfWork.Users.Add(user);
        await _identityUnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}