using Inventory.Product.Services.Models.Requests.InventoryEntry;
using Inventory.Product.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Inventory.Product.API.Controllers;

[ApiController]
[Route("v1/inventory-entries")]
public class InventoryEntryController : ControllerBase
{
    private readonly IInventoryEntryService _inventoryEntryService;

    public InventoryEntryController(IInventoryEntryService inventoryEntryService)
    {
        _inventoryEntryService = inventoryEntryService;
    }

    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetInventoryEntries([FromQuery] GetInventoryEntryRequest request)
    {
        var result = await _inventoryEntryService.GetInventoryEntries(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetInventoryEntryById(string id)
    {
        var result = await _inventoryEntryService.GetInventoryEntryById(id);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> CreateInventoryEntry([FromBody] PurchaseProductRequest request)
    {
        var result = await _inventoryEntryService.PurchaseProduct(request);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}