using Inventory.Product.Application.Usecases.InventoryEntry.Command.PurchaseProduct;
using Inventory.Product.Application.Usecases.InventoryEntry.Query.GetInventoryEntryById;
using Inventory.Product.Application.Usecases.InventoryEntry.Query.GetListInventoryEntries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Inventory.Product.Presentation.Controllers;

[ApiController]
public class InventoryEntryController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [Route("v1/inventory-entries")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetInventoryEntries([FromQuery] GetListInventoryEntriesQuery request)
    {
        var result = await Sender.Send(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpGet]
    [Route("v1/inventory-entries/{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetInventoryEntryById(string id)
    {
        var result = await Sender.Send(new GetInventoryEntryByIdQuery(id));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpPost]
    [Route("v1/inventory-entries/purchase")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> CreateInventoryEntry([FromBody] PurchaseProductCommand request)
    {
        var result = await Sender.Send(request);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}