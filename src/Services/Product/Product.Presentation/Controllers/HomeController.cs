using Microsoft.AspNetCore.Mvc;

namespace Product.Presentation.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}