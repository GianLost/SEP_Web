using Microsoft.AspNetCore.Mvc;
using SEP_Web.Filters;

namespace SEP_Web.Controllers;

[LoggedinUserFilter]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();
}
