using Basket.Services.Models.Requests.Checkout;
using Basket.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var res = await _checkoutService.CheckoutAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}