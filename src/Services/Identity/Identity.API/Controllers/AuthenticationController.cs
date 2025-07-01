using Identity.Application.Usecases.Authentication.Login;
using Identity.Application.Usecases.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(ISender sender) : ApiController(sender)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}