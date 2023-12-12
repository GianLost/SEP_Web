using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using SEP_Web.Helper.Validation;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

public class CivilServantController : Controller
{
    private readonly ILogger<CivilServantController> _logger;
    private readonly IUsersValidation _validation;
    private readonly ICivilServantServices _civilServantServices;

    public CivilServantController(ILogger<CivilServantController> logger, IUsersValidation validation, ICivilServantServices civilServantServices)
    {
        _logger = logger;
        _validation = validation;
        _civilServantServices = civilServantServices;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<CivilServant> users = await _civilServantServices.ServantsList();
            return View(users);
        }
        catch (MySqlException ex)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorServantList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<CivilServant>());
        }
        catch (ArgumentNullException ex2)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<CivilServant>());
        }
    }

    public IActionResult Register() => View();
    
    [HttpPost]
    public async Task<IActionResult> Register(CivilServant servant, string confirmPass)
    {

        try
        {

            if (string.IsNullOrEmpty(confirmPass))
            {
                TempData["ErrorPass"] = FeedbackMessages.ConfirmPassword;
                return View(servant);
            }

            if (ModelState.IsValid)
            {
                List<(string FieldName, string Message)> duplicateErrors = await _validation.CheckForDuplicateDatatableFields(servant);

                foreach (var (FieldName, Message) in duplicateErrors)
                    ModelState.AddModelError(FieldName, Message);

                if (ModelState.ErrorCount > 0)
                    return View(servant);

                if (!_validation.ValidatePassword(servant.Password, confirmPass, this))
                    return View(servant);

                /* Assim que todos os dados forem validados de acordo com as exigências; */

                TempData["SuccessMessage"] = FeedbackMessages.SuccessServantRegister;

                await _civilServantServices.RegisterServant(servant);
                return RedirectToAction("Index");
            }

            return View(servant);
        }
        catch (MySqlException ex)
        {
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorServantRegister} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);

            _logger.LogError("{exceptionMessage} : {Description} - ", ExceptionMessages.ErrorUnexpected, ex.StackTrace.Trim());

            servant = null;
            return View(servant);
        }
        catch (Exception ex2)
        {
            TempData["ErrorMessage"] = FeedbackMessages.ErrorServantRegister;

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorUnexpected, ex2.Message, ex2.InnerException);

            _logger.LogWarning("{exceptionMessage} : {Description}", ExceptionMessages.ErrorUnexpected, ex2.StackTrace.Trim());

            servant = null;
            return View(servant);
        }
    }
}
