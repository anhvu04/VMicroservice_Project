using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Services.Models.Requests.CatalogProduct;
using Product.Services.Services.Interfaces;
using Shared.Enums;

namespace Product.API.Controllers;

[ApiController]
[Route("v1/products")]
public class CatalogProductController : ControllerBase
{
    private readonly ICatalogProductService _catalogProductService;

    public CatalogProductController(ICatalogProductService catalogProductService)
    {
        _catalogProductService = catalogProductService;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetCatalogProductsRequest request)
    {
        var res = await _catalogProductService.GetCatalogProductsAsync(request);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var res = await _catalogProductService.GetCatalogProductAsync(id);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateCatalogProductRequest request)
    {
        var res = await _catalogProductService.CreateCatalogProductAsync(request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateCatalogProductRequest request)
    {
        var res = await _catalogProductService.UpdateCatalogProductAsync(id, request);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var res = await _catalogProductService.DeleteCatalogProductAsync(id);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}