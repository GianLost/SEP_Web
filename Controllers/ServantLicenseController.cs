using Microsoft.AspNetCore.Mvc;

namespace SEP_Web.Controllers;
public class ServantLicenseController : Controller
{
    private readonly ILogger<ServantLicenseController> _logger;

    public ServantLicenseController(ILogger<ServantLicenseController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index() => View();

    public IActionResult UnderLicense() => View();

}
