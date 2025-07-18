using Basket.Presentation.Extensions;
using Basket.Application.Abstractions;
using Basket.Application.Usecases.Cart.Command.AddCart;
using Basket.Application.Usecases.Cart.Command.RemoveCart;
using Basket.Application.Usecases.Cart.Command.UpdateCart;
using Basket.Application.Usecases.Cart.Query.GetCart;
using Infrastructure.Extensions.JwtExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Basket.Presentation.Controllers;

[ApiController]
[Route("v1/carts")]
public class CartController(ISender sender) : ApiController(sender)
{
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

        var res = await Sender.Send(new GetCartQuery(validateClaims.UserId));
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> AddItemToCart([FromBody] AddToCartCommand command)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        command.UserId = validateClaims.UserId;
        var res = await Sender.Send(command);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpPatch]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> UpdateItemInCart([FromBody] UpdateToCartCommand request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await Sender.Send(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpDelete]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> DeleteItemFromCart([FromBody] RemoveFromCartCommand request)
    {
        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        request.UserId = validateClaims.UserId;
        var res = await Sender.Send(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}