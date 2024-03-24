using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

[ServiceFilter(typeof(UserEvaluatorFilter))]
public class ServantLicenseController : Controller
{
    private readonly ILogger<ServantLicenseController> _logger;
    private readonly ILicenseServices _licenses;
    private readonly IServantLicenseServices _servantLicenses;

    public ServantLicenseController(ILogger<ServantLicenseController> logger, ILicenseServices licenses, IServantLicenseServices servantLicenses)
    {
        _logger = logger;
        _licenses = licenses;
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
    public async Task<IActionResult> Register([Bind("CivilServantId, LicensesId, StartDate, EndDate")]ServantLicense servantLicense)
    {
        try
        {
            if (ModelState.IsValid)
            {
                int maxDuration = await _licenses.GetMaxLicenseDuration(servantLicense.LicensesId);

                // Verificar se a diferença entre as datas excede a duração máxima
                TimeSpan? duration = servantLicense.EndDate - servantLicense.StartDate;
                
                if (duration.Value.Days > maxDuration)
                {
                    TempData["ErrorMessage"] = "O prazo estabelecido excede o tempo máximo permitido para a licença selecionada !";
                    return Json(new { stats = StatsAJAXEnum.ERROR_TIME});
                }

                if (servantLicense.EndDate <= servantLicense.StartDate)
                {
                    TempData["ErrorMessage"] = "A data final da licença não pode ser anterior a data de início !";
                    return Json(new { stats = StatsAJAXEnum.INVALID_TIME});
                }else {
                    TempData.Clear();
                }

                await _servantLicenses.RegisterServantLicense(servantLicense);

                TempData["SuccessMessage"] = "Licença adicionada para o servidor.";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            return Json(new { stats = StatsAJAXEnum.ERROR, message = "Não foi possível adicionar a licença!" });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError("Não foi possível adicionar a licença. Error : {Message}", e.Message);
            return Json(new { stats = StatsAJAXEnum.INVALID, message = "Não foi possível adicionar a liceça!" });
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
            _logger.LogError("Não foi possível excluir a licença. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult GetLicenseDuration(int id)
    {
        try
        {
            Licenses license = _licenses.SearchForId(id);

            if (license != null)
                return Ok(license.Time);
            
            return NotFound();
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogError("Não foi possível capturar a licença. Error : {Message}", ex.Message);
            return StatusCode(500);
        }
    }

}
