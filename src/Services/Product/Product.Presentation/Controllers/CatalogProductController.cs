using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Usecases.CatalogProduct.Command.CreateCatalogProduct;
using Product.Application.Usecases.CatalogProduct.Command.DeleteCatalogProduct;
using Product.Application.Usecases.CatalogProduct.Command.UpdateCatalogProduct;
using Product.Application.Usecases.CatalogProduct.Query.GetCatalogProductById;
using Product.Application.Usecases.CatalogProduct.Query.GetListCatalogProducts;
using Shared.Enums;

namespace Product.Presentation.Controllers;

[ApiController]
[Route("v1/products")]
public class CatalogProductController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetListCatalogProductsQuery request)
    {
        var res = await Sender.Send(request);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var res = await Sender.Send(new GetCatalogProductByIdQuery(id));
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateCatalogProductCommand command)
    {
        var res = await Sender.Send(command);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateCatalogProductCommand command)
    {
        command.Id = id;
        var res = await Sender.Send(command);
        return res.IsSuccess ? Ok() : BadRequest(res);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        var res = await Sender.Send(new DeleteCatalogProductCommand(id));
        return res.IsSuccess ? Ok() : BadRequest(res);
    }
}