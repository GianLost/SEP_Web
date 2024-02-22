using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

public class LicenseController : Controller
{
    private readonly ILogger<LicenseController> _logger;
    private readonly IUserSession _session;
    private readonly ILicenseServices _licenses;

    public LicenseController(ILogger<LicenseController> logger, IUserSession session, ILicenseServices licenses)
    {
        _logger = logger;
        _session = session;
        _licenses = licenses;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<Licenses> license = await _licenses.LicenseList();
            if (license == null)
                throw new ArgumentNullException(nameof(license), ExceptionMessages.ErrorArgumentNullException);

            if (license?.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            return View(license ?? new List<Licenses>());
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorDivisionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<Licenses>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<Licenses>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<Licenses>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Licenses license)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                license.UserAdministratorId = userInSession.Id;

                await _licenses.RegisterLicense(license);
                TempData["SuccessMessage"] = "Liceça cadastrada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadsatrar a licença!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar a licença", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar a liceça!" });
        }
    }
    

    public IActionResult UnderLicense() => View();
    
}