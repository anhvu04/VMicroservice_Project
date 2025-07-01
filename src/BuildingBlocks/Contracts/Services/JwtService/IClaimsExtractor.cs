using System.Security.Claims;

namespace Contracts.Services.JwtService;

public interface IClaimsExtractor
{
    Guid? ExtractUserId(ClaimsPrincipal principal);
}