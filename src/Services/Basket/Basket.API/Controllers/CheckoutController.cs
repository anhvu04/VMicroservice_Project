using Basket.Services.Models.Requests.Checkout;
using Basket.Services.Services.Interfaces;
using Infrastructure.Extensions.JwtExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Basket.API.Controllers;

[ApiController]
[Route("v1/checkout")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await _checkoutService.CheckoutAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}