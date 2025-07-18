using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extensions.JwtExtensions
{
    public class UserClaimsPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private const string PolicyPrefix = "UserClaims_";

        public UserClaimsPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PolicyPrefix) &&
                Enum.TryParse<ClaimRequirements>(policyName.Substring(PolicyPrefix.Length), out var requirements))
            {
                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new UserClaimsRequirement(requirements))
                    .Build();

                return Task.FromResult(policy)!;
            }

            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}