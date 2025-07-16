using Basket.Application.Usecases.Checkout.Command;
using Infrastructure.Extensions.JwtExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Basket.Presentation.Controllers;

[ApiController]
[Route("v1/checkout")]
public class CheckoutController(ISender sender) : ApiController(sender)
{
    // private static int _count = 0; // For testing circuit breaker

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Customer))]
    [RequireUserClaims(ClaimRequirements.UserId)]
    public async Task<IActionResult> Checkout([FromBody] CheckoutCommand command)
    {
        // For testing circuit breaker
        // _count++;
        // Console.WriteLine(_count);
        // if (_count < 4)
        // {
        //     // Force an exception to clearly trigger the circuit breaker
        //     throw new Exception($"Test exception for circuit breaker - count: {_count}");
        // }

        var validateClaims = this.GetValidatedUserClaims();
        if (validateClaims is null)
        {
            return BadRequest(new { message = "Invalid user claims" });
        }

        command.UserId = validateClaims.UserId;
        var res = await Sender.Send(command);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}