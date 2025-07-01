using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions.JwtExtensions
{
    public class UserClaimsHandler : AuthorizationHandler<UserClaimsRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            UserClaimsRequirement requirement)
        {
            var claimsValidator = context.User.ValidateClaims();
            
            if (requirement.RequiredClaims.HasFlag(ClaimRequirements.UserId))
                claimsValidator.RequireUserId();
                
            if (requirement.RequiredClaims.HasFlag(ClaimRequirements.PhoneNumber))
                claimsValidator.RequirePhoneNumber();
                
            if (requirement.RequiredClaims.HasFlag(ClaimRequirements.Email))
                claimsValidator.RequireEmail();
                
            if (requirement.RequiredClaims.HasFlag(ClaimRequirements.Role))
                claimsValidator.RequireRole();
            
            if (requirement.RequiredClaims.HasFlag(ClaimRequirements.FullName))
                claimsValidator.RequireFullName();
            
            var validateResult = claimsValidator.Validate();
            
            if (validateResult.IsSuccess && validateResult.Value != null)
            {
                // Store validated claims in HttpContext for later use
                if (_httpContextAccessor.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Items["ValidatedUserClaims"] = validateResult.Value;
                }
                
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}