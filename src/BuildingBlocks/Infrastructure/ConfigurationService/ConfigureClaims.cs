using Infrastructure.Extensions.JwtExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ConfigurationService;

public static class ConfigureClaims
{
    public static void ConfigureClaimsRequirement(this WebApplicationBuilder builder)
    {
        // Add services
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IAuthorizationHandler, UserClaimsHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, UserClaimsPolicyProvider>();
    }
}