using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Usecases.Order.Command.CreateOrder;
using Ordering.Application.Usecases.Order.Command.DeleteOrder;
using Ordering.Application.Usecases.Order.Query.GetOrder;
using Ordering.Application.Usecases.Order.Query.GetOrders;
using Shared.Enums;

namespace Ordering.Presentation.Controllers;

[ApiController]
[Route("v1/orders")]
public class OrderController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] GetOrdersQuery query)
    {
        var result = await Sender.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> GetOrdersAsync([FromRoute] Guid id)
    {
        var query = new GetOrderQuery(id);
        var result = await Sender.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    /// <summary>
    /// This is for testing purposes only.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommand command)
    {
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }

    /// <summary>
    /// This is for testing purposes only.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = nameof(UserRoles.Admin) + "," + nameof(UserRoles.Staff))]
    public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid id)
    {
        var result = await Sender.Send(new DeleteOrderCommand(id));
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}