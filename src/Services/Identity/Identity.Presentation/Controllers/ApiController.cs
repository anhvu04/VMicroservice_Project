using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[ApiController]
public class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;
}