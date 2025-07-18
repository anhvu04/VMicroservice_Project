using Microsoft.AspNetCore.Mvc;

namespace Customer.Presentation.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}