using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

public class LicenseController : Controller
{
    private readonly ILogger<LicenseController> _logger;
    private readonly ILicenseServices _licenses;

    public LicenseController(ILogger<LicenseController> logger, ILicenseServices licenses)
    {
        _logger = logger;
        _licenses = licenses;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<Licenses> license = await _licenses.LicenseList();
            return View(license);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorAdministratorList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<UserAdministrator>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<UserAdministrator>());
        }
    }
    

    public IActionResult UnderLicense() => View();
    
}