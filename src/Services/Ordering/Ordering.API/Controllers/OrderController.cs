using Contracts.Services.EmailService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Usecases.Order.Query.GetOrder;
using Ordering.Application.Usecases.Order.Query.GetOrders;
using Shared.Services.EmailService;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(ISender sender, ISmtpEmailService emailService) : ApiController(sender)
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