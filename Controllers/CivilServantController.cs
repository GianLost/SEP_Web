using Microsoft.AspNetCore.Mvc;

namespace SEP_Web.Controllers;

public class CivilServantController : Controller
{
    private readonly ILogger<CivilServantController> _logger;

    public CivilServantController(ILogger<CivilServantController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
