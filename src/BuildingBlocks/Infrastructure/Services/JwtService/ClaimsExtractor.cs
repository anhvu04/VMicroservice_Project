using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Contracts.Services.JwtService;

namespace Infrastructure.Services.JwtService;

public class ClaimsExtractor : IClaimsExtractor
{
    public Guid? ExtractUserId(ClaimsPrincipal principal)
    {
        var claims = principal.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null) return null;
        if (Guid.TryParse(userIdClaim, out var userId)) return userId;
        return null;
    }
}