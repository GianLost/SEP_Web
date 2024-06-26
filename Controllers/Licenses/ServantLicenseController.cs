using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models.LicensesModels;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.LicensesInterfaces;
using SEP_Web.Interfaces.AssessmentsInterfaces;

namespace SEP_Web.Controllers.LicensesController;

[ServiceFilter(typeof(UserAdminFilter))]
public class ServantLicenseController : Controller
{
    private readonly ILogger<ServantLicenseController> _logger;
    private readonly ILicenseServices _licenses;
    private readonly IServantLicenseServices _servantLicenses;
    private readonly IAssessmentServices _assessmentServices;
    private readonly IUserSession _session;

    public ServantLicenseController(ILogger<ServantLicenseController> logger, ILicenseServices licenses, IServantLicenseServices servantLicenses,IAssessmentServices assessmentServices, IUserSession session)
    {
        _logger = logger;
        _licenses = licenses;
        _servantLicenses = servantLicenses;
        _assessmentServices = assessmentServices;
        _session = session;
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
                int maxDuration = await _licenses.GetMaxLicenseDuration(servantLicense.LicensesId);

                // Verificar se a diferença entre as datas excede a duração máxima
                TimeSpan? duration = servantLicense.EndDate - servantLicense.StartDate;

                if (duration.Value.Days > maxDuration)
                    return Json(new { stats = StatsAJAXEnum.ERROR_TIME });

                if (servantLicense.EndDate <= servantLicense.StartDate)
                    return Json(new { stats = StatsAJAXEnum.INVALID_TIME });

                await _servantLicenses.RegisterServantLicense(servantLicense);

                TempData["SuccessMessage"] = $" A Licença para o(a) servidor(a) {await _assessmentServices.ServantName(servantLicense.CivilServantId)} foi cadastrada com sucesso !";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            TempData["ErrorMessage"] = "Erro ao adicionar licença para o servidor.";
            return Json(new { stats = StatsAJAXEnum.ERROR, message = "Não foi possível adicionar a licença!" });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError("Não foi possível adicionar a licença. Error : {Message}", e.Message);
            return Json(new { stats = StatsAJAXEnum.INVALID, message = "Não foi possível adicionar a liceça!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ServantLicense servantLicense)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                int maxDuration = await _licenses.GetMaxLicenseDuration(servantLicense.LicensesId);

                // Verificar se a diferença entre as datas excede a duração máxima
                TimeSpan? duration = servantLicense.EndDate - servantLicense.StartDate;

                if (duration.Value.Days > maxDuration)
                    return Json(new { stats = StatsAJAXEnum.ERROR_TIME });

                if (servantLicense.EndDate <= servantLicense.StartDate)
                    return Json(new { stats = StatsAJAXEnum.INVALID_TIME });

                servantLicense.LastModifiedBy = userInSession.Login;

                await _servantLicenses.ServantLicensesEdit(servantLicense);

                TempData["SuccessMessage"] = $" A Licença do(a) servidor(a) {await _assessmentServices.ServantName(servantLicense.CivilServantId)} foi editada com sucesso !";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            TempData["ErrorMessage"] = "Erro ao editar licença para o servidor.";
            return Json(new { stats = StatsAJAXEnum.ERROR, civilServantId = servantLicense.CivilServantId, licenseName = servantLicense.LicensesId, sDate = servantLicense.StartDate, eDate = servantLicense.EndDate });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível editar a licença. Error : {Message}", e.Message);
            TempData["ErrorMessage"] = "Erro interno ao editar a licença!";
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
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Não foi possível capturar a licença. Error : {Message}", ex.Message);
            return StatusCode(500);
        }
    }

}