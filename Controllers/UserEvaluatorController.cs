using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP_Web.Filters;
using SEP_Web.Models;
using SEP_Web.Services;
using MySqlConnector;
using SEP_Web.Helper.Validation;
using SEP_Web.Helper.Messages;

namespace SEP_Web.Controllers;

[UserEvaluatorFilter]
public class UserEvaluatorController : Controller
{
    private readonly ILogger<UserEvaluatorController> _logger;
    private readonly IUserEvaluatorServices _evaluatorServices;
    private readonly IDivisionServices _divisionServices;
    private readonly ISectionServices _sectionServices;
    private readonly ISectorServices _sectorServices;
    private readonly IUsersValidation _validation;

    public UserEvaluatorController(ILogger<UserEvaluatorController> logger, IUserEvaluatorServices evaluatorServices, IDivisionServices divisionServices, ISectorServices sectorServices, ISectionServices sectionServices, IUsersValidation validation)
    {
        _logger = logger;
        _evaluatorServices = evaluatorServices;
        _divisionServices = divisionServices;
        _sectionServices = sectionServices;
        _sectorServices = sectorServices;
        _validation = validation;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<UserEvaluator> users = await _evaluatorServices.EvaluatorsList();
            return View(users);
        }
        catch (MySqlException ex)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<UserEvaluator>());
        }
        catch (ArgumentNullException ex2)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<UserEvaluator>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetDivisionsByInstituition(int instituitionId)
    {
        ICollection<Division> divisions = await _divisionServices.GetDivisionsAsync(instituitionId);

        IEnumerable<SelectListItem> divisionList = divisions.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(divisionList);
    }

    [HttpGet]
    public async Task<IActionResult> GetSectionsByDivisions(int divisionId)
    {
        ICollection<Section> sections = await _sectionServices.GetSectionsAsync(divisionId);

        IEnumerable<SelectListItem> sectionList = sections.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(sectionList);
    }

    [HttpGet]
    public async Task<IActionResult> GetSectorsBySections(int sectionId)
    {
        ICollection<Sector> sectors = await _sectorServices.GetSectorsAsync(sectionId);

        IEnumerable<SelectListItem> sectorList = sectors.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(sectorList);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserEvaluator evaluator, string confirmPass)
    {

        try
        {

            if (string.IsNullOrEmpty(confirmPass))
            {
                TempData["ErrorPass"] = FeedbackMessages.ConfirmPassword;
                return View(evaluator);
            }

            if (ModelState.IsValid)
            {
                List<(string FieldName, string Message)> duplicateErrors = await _validation.CheckForDuplicateDatatableFields(evaluator);

                foreach (var (FieldName, Message) in duplicateErrors)
                    ModelState.AddModelError(FieldName, Message);

                if (ModelState.ErrorCount > 0)
                    return View(evaluator);

                if (!_validation.ValidatePassword(evaluator.Password, confirmPass, this))
                    return View(evaluator);

                /* Assim que todos os dados forem validados de acordo com as exigências; */

                TempData["SuccessMessage"] = FeedbackMessages.SuccessEvaluatorRegister;

                await _evaluatorServices.RegisterEvaluator(evaluator);
                return RedirectToAction("Index");
            }

            return View(evaluator);
        }
        catch (MySqlException ex)
        {
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorRegister} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);

            _logger.LogError("{exceptionMessage} : {Description} - ", ExceptionMessages.ErrorUnexpected, ex.StackTrace.Trim());

            evaluator = null;
            return View(evaluator);
        }
        catch (Exception ex2)
        {
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEvaluatorRegister;

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorUnexpected, ex2.Message, ex2.InnerException);

            _logger.LogWarning("{exceptionMessage} : {Description}", ExceptionMessages.ErrorUnexpected, ex2.StackTrace.Trim());

            evaluator = null;
            return View(evaluator);
        }
    }

}
