using System.Security.Claims;
using Shared.Utils;

namespace Infrastructure.Extensions.JwtExtensions;

public class ClaimsValidatorBuilder
{
    private readonly ClaimsPrincipal? _principal;
    private readonly List<Func<ClaimsPrincipal?, Result>> _validators = new();
    private readonly ClaimsData _claimsData = new();

    public ClaimsValidatorBuilder(ClaimsPrincipal? principal)
    {
        _principal = principal;
    }

    public ClaimsValidatorBuilder RequireUserId()
    {
        _validators.Add(p =>
        {
            var userId = p.GetUserId();
            if (userId == null || userId == Guid.Empty)
                return Result.Failure("User ID is required");

            _claimsData.UserId = userId.Value;
            return Result.Success();
        });
        return this;
    }

    public ClaimsValidatorBuilder RequireEmail()
    {
        _validators.Add(p =>
        {
            var email = p.GetUserEmail();
            if (string.IsNullOrEmpty(email))
                return Result.Failure("Email is required");

            _claimsData.Email = email;
            return Result.Success();
        });
        return this;
    }

    public ClaimsValidatorBuilder RequireRole()
    {
        _validators.Add(p =>
        {
            var role = p.GetRole();
            if (string.IsNullOrEmpty(role))
                return Result.Failure("Role is required");

            _claimsData.Role = role;
            return Result.Success();
        });
        return this;
    }

    public ClaimsValidatorBuilder RequireFullName()
    {
        _validators.Add(p =>
        {
            var fullName = p.GetFullName();
            if (string.IsNullOrEmpty(fullName))
                return Result.Failure("Full name is required");

            _claimsData.FullName = fullName;
            return Result.Success();
        });
        return this;
    }

    public ClaimsValidatorBuilder RequirePhoneNumber()
    {
        _validators.Add(p =>
        {
            var phoneNumber = p.GetPhoneNumber();
            if (string.IsNullOrEmpty(phoneNumber))
                return Result.Failure("Phone number is required");

            _claimsData.PhoneNumber = phoneNumber;
            return Result.Success();
        });
        return this;
    }

    public Result<ClaimsData> Validate()
    {
        foreach (var validator in _validators)
        {
            var result = validator(_principal);
            if (result.IsSuccess) continue;
            if (result.Error != null) return Result.Failure<ClaimsData>(result.Error);
        }

        return Result.Success(_claimsData);
    }

    public ClaimsData GetClaimsData()
    {
        return _claimsData;
    }
}

public class ClaimsData
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}