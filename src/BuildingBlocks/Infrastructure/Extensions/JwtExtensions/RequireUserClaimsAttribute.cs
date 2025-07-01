using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Extensions.JwtExtensions
{
    public class RequireUserClaimsAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "UserClaims_";

        public RequireUserClaimsAttribute(ClaimRequirements requirements)
        {
            Policy = $"{PolicyPrefix}{requirements}";
        }
    }
}