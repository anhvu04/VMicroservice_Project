using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Extensions.JwtExtensions
{
    public static class ControllerExtensions
    {
        public static ClaimsData? GetValidatedUserClaims(this ControllerBase controller)
        {
            return controller.HttpContext.Items["ValidatedUserClaims"] as ClaimsData;
        }
    }
}