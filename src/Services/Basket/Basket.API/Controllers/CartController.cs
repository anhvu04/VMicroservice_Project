using Basket.Services.Models.Requests.Cart;
using Basket.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart([FromRoute] Guid userId)
    {
        var res = await _cartService.GetCartAsync(userId, new GetCartRequest());
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequest request)
    {
        var res = await _cartService.AddToCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateItemInCart([FromBody] UpdateToCartRequest request)
    {
        var res = await _cartService.UpdateToCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteItemFromCart([FromBody] RemoveFromCartRequest request)
    {
        var res = await _cartService.RemoveFromCartAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}