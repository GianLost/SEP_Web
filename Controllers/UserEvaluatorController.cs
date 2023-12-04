using Microsoft.AspNetCore.Mvc;

namespace SEP_Web.Controllers;

public class UserEvaluatorController : Controller
{
    private readonly ILogger<UserEvaluatorController> _logger;

    public UserEvaluatorController(ILogger<UserEvaluatorController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
}
