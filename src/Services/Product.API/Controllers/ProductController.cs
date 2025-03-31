using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok();
    }
}