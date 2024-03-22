using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Helper.Validation;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

[UserEvaluatorFilter]
public class CivilServantController : Controller
{
    private readonly ILogger<CivilServantController> _logger;
    private readonly IUsersValidation _validation;
    private readonly IUserSession _session;
    private readonly ICivilServantServices _civilServantServices;
    private readonly IAssessmentServices _assessmentServices;

    public CivilServantController(ILogger<CivilServantController> logger, IUserSession session, IUsersValidation validation, ICivilServantServices civilServantServices, IAssessmentServices assessmentServices)
    {
        _logger = logger;
        _validation = validation;
        _session = session;
        _civilServantServices = civilServantServices;
        _assessmentServices = assessmentServices;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<CivilServant> users = await _civilServantServices.ServantsList();
            if (users?.Count == 0)
                throw new ArgumentNullException(nameof(users), ExceptionMessages.ErrorArgumentNullException);
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
                List<(string FieldName, string Message)> duplicateErrors = await _validation.CheckForDuplicateUserFields(servant);

                foreach (var (FieldName, Message) in duplicateErrors)
                    ModelState.AddModelError(FieldName, Message);

                    if (ModelState.ErrorCount > 0)
                        return View(servant);

                        if (!_validation.ValidatePassword(servant.Password, confirmPass, this))
                            return View(servant);

                TempData["SuccessMessage"] = FeedbackMessages.SuccessServantRegister;

                await _civilServantServices.RegisterServant(servant);

                DateTime evaluationDate = DateTime.Now.AddMonths(7); 

                for(int stage = 1; stage <= 5; stage++)
                {
                    
                    Assessment newTest = new ()
                    {
                        Stats = AssessmentStatsEnum.NOT_EVALUATED,
                        Phase = stage,
                        StartEvaluationPeriod = stage == 1 ? servant.AdmissionDate.AddDays(210) : evaluationDate,
                        CivilServantId = servant.Id,
                        UserEvaluatorId1 = servant.UserEvaluatorId1,
                        UserEvaluatorId2 = servant.UserEvaluatorId2,
                        EvaluatedFor = null
                    };

                    evaluationDate = Convert.ToDateTime(newTest.StartEvaluationPeriod).AddDays(210);

                    await _assessmentServices.RegisterAssessments(newTest);
                }

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

    public IActionResult Edit(int id) => View(_civilServantServices.SearchForId(id));

    [HttpPost]
    public async Task<IActionResult> Edit(ModifyUser modifyUser)
    {
        try
        {
            CivilServant userEdit = _civilServantServices.SearchForId(modifyUser.Id);

            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();

                bool enableAccount = Request.Form["enableDisableAccount"] == "on";

                if (enableAccount)
                {
                    userEdit.UserStats = userEdit.UserStats == UserStatsEnum.Active ? UserStatsEnum.Inactive : UserStatsEnum.Active;
                }

                var fieldsToValidate = new List<(string FieldName, object Value)>
                {
                    ("Masp", modifyUser.Masp),
                    ("Name", modifyUser.Name),
                    ("Login", modifyUser.Login),
                    ("Email", modifyUser.Email),
                    ("Phone", modifyUser.Phone),
                };

                foreach (var (fieldName, value) in fieldsToValidate)
                    if (_validation.IsFieldChanged(userEdit, fieldName, value))
                        if (await _validation.CheckIfFieldExistsInAnyUserTable(fieldName, value))
                            ModelState.AddModelError(fieldName, $"O {fieldName.ToLower()} informado já está em uso.");

                if (ModelState.ErrorCount > 0)
                    return View(userEdit);

                userEdit = new CivilServant()
                {
                    Id = modifyUser.Id,
                    Masp = modifyUser.Masp,
                    Name = modifyUser.Name,
                    Login = modifyUser.Login,
                    Email = modifyUser.Email,
                    Phone = modifyUser.Phone,
                    Position = modifyUser.Position,
                    UserStats = userEdit.UserStats,
                    LastModifiedBy = userInSession.Login
                };

                TempData["SuccessMessage"] = FeedbackMessages.SuccessServantEdit;

                await _civilServantServices.ServantsEdit(userEdit);
                return RedirectToAction("Index");
            }

            return View(userEdit);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorEdit} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[SERVANT_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[SERVANT_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorServantEdit;

            _logger.LogWarning("[SERVANT_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[SERVANT_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, CivilServant user)
    {
        try
        {
            if (decision == "delete")
            {
                if (user.Id != 0)
                {
                    TempData["SuccessMessage"] = FeedbackMessages.SuccessServantDelete;

                    _civilServantServices.DeleteServant(user.Id);
                    return RedirectToAction("Index");
                }
            }

            throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorDelete} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[SERVANT_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[SERVANT_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorEvaluatorDelete; // Mensagem de vizualização para o usuário;

            _logger.LogWarning("[SERVANT_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[SERVANT_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
    }
}
