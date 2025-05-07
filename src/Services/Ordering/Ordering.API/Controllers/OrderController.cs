using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Usecases.Order.Query.GetOrder;
using Ordering.Application.Usecases.Order.Query.GetOrders;

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
}