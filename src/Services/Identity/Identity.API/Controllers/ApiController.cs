using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
public class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;
}