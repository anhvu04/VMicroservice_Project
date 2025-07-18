using Identity.Domain.Entities;
using Shared.Utils.Errors;

namespace Identity.Domain.DomainErrors;

public static class UserErrors
{
    public static readonly string UserNotFound = CommonErrors.NotFound<User>();
    public static readonly string UserAlreadyExists = CommonErrors.AlreadyExists<User>();
    public static readonly string WrongPassword = "Wrong username or password.";
}