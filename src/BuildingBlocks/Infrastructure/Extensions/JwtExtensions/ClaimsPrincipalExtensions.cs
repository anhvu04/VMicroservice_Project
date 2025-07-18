using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Infrastructure.Extensions.JwtExtensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal? principal)
    {
        return principal != null && principal.IsInRole(nameof(UserRoles.Admin));
    }

    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        var userIdClaim = principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
    }

    public static string? GetUserEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }

    public static string? GetRole(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("role")?.Value;
    }

    public static string? GetAccessToken(this HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return null;
        }

        return authHeader["Bearer ".Length..];
    }

    public static string? GetFullName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("full_name")?.Value;
    }

    public static string? GetPhoneNumber(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("phone_number")?.Value;
    }

    public static ClaimsValidatorBuilder ValidateClaims(this ClaimsPrincipal? principal)
    {
        return new ClaimsValidatorBuilder(principal);
    }
}