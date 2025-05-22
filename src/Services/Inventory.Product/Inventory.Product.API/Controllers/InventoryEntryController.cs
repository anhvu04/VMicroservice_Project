using Inventory.Product.Services.Models.Requests.InventoryEntry;
using Inventory.Product.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Product.API.Controllers;

[ApiController]
[Route("api/inventory-entry")]
public class InventoryEntryController : ControllerBase
{
    private readonly IInventoryEntryService _inventoryEntryService;

    public InventoryEntryController(IInventoryEntryService inventoryEntryService)
    {
        _inventoryEntryService = inventoryEntryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventoryEntries([FromQuery] GetInventoryEntryRequest request)
    {
        var result = await _inventoryEntryService.GetInventoryEntries(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }
}