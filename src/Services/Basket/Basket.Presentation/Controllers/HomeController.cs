using Microsoft.AspNetCore.Mvc;

namespace Basket.Presentation.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}