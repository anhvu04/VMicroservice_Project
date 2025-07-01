using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Extensions.JwtExtensions
{
    [Flags]
    public enum ClaimRequirements
    {
        None = 0,
        UserId = 1 << 0,
        PhoneNumber = 1 << 1,
        Email = 1 << 2,
        Role = 1 << 3,
        FullName = 1 << 4,
        // Add more as needed
    }

    public class UserClaimsRequirement : IAuthorizationRequirement
    {
        public ClaimRequirements RequiredClaims { get; }

        public UserClaimsRequirement(ClaimRequirements requiredClaims)
        {
            RequiredClaims = requiredClaims;
        }
    }
}