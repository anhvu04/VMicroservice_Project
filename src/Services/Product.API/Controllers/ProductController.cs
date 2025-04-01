using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Product.Services.Services.Interfaces;

namespace Product.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly ICatalogProductService _catalogProductService;

    public ProductController(ILogger<ProductController> logger, ICatalogProductService catalogProductService)
    {
        _logger = logger;
        _catalogProductService = catalogProductService;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await _catalogProductService.GetProductsAsync());
    }
}