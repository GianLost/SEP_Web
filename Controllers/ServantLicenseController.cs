using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;
public class ServantLicenseController : Controller
{
    private readonly ILogger<ServantLicenseController> _logger;
    private readonly IUserSession _session;
    private readonly IServantLicenseServices _servantLicenses;

    public ServantLicenseController(ILogger<ServantLicenseController> logger, IUserSession session, IServantLicenseServices servantLicenses)
    {
        _logger = logger;
        _session = session;
        _servantLicenses = servantLicenses;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<ServantLicense> servantLicense = await _servantLicenses.ServantLicenseList();
            if (servantLicense == null)
                throw new ArgumentNullException(nameof(servantLicense), ExceptionMessages.ErrorArgumentNullException);

            if (servantLicense?.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            return View(servantLicense ?? new List<ServantLicense>());
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorDivisionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<ServantLicense>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<ServantLicense>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<ServantLicense>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(ServantLicense servantLicense)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _servantLicenses.RegisterServantLicense(servantLicense);
                TempData["SuccessMessage"] = "Licença adicionada para o servidor.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível adicionar a licença!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível adicionar a licença", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível adicionar a liceça!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, ServantLicense servantLicense)
    {

        try
        {
            if (decision == "delete")
            {
                if (servantLicense.Id != 0)
                {
                    _servantLicenses.DeleteServantLicenses(servantLicense.Id);
                    TempData["SuccessMessage"] = "Licença excluída com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir a licença.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir a licença.";
            _logger.LogError("Não foi possível excluir a licença", e.Message);
            return RedirectToAction("Index");
        }

    }

}
