using Basket.API.Extensions;
using Basket.Services.Models.Requests.Cart;
using Basket.Services.Services.Interfaces;
using Infrastructure.Extensions.JwtExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Basket.API.Controllers;

[ApiController]
[Route("v1/carts")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> GetCart()
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        var res = await _cartService.GetCartAsync(validateClaims.UserId);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequest request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await _cartService.AddToCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpPatch]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> UpdateItemInCart([FromBody] UpdateToCartRequest request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await _cartService.UpdateToCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpDelete]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> DeleteItemFromCart([FromBody] RemoveFromCartRequest request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await _cartService.RemoveFromCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}