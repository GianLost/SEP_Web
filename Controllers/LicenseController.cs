using Microsoft.AspNetCore.Mvc;


namespace SEP_Web.Controllers;

public class LicenseController : Controller
{
    private readonly ILogger<LicenseController> _logger;

    public LicenseController(ILogger<LicenseController> logger)
    {
        _logger = logger;
    }

    public IActionResult LicenseType()
    {
        return View();
    }

    public IActionResult UnderLicense()
    {
        return View();
    }
}