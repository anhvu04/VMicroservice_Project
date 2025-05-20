using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Usecases.Order.Command.CreateOrder;
using Ordering.Application.Usecases.Order.Command.DeleteOrder;
using Ordering.Application.Usecases.Order.Query.GetOrder;
using Ordering.Application.Usecases.Order.Query.GetOrders;
using Shared.Services.EmailService;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] GetOrdersQuery query)
    {
        var result = await Sender.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrdersAsync([FromRoute] Guid id)
    {
        var query = new GetOrderQuery(id);
        var result = await Sender.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommand command)
    {
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid id)
    {
        var result = await Sender.Send(new DeleteOrderCommand(id));
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}