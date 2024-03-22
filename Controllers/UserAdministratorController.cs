using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Helper.Validation;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web_.Controllers;

[ServiceFilter(typeof(UserAdminFilter))]
public class UserAdministratorController : Controller
{
    private readonly ILogger<UserAdministratorController> _logger;
    private readonly IUserAdministratorServices _administratorServices;
    private readonly IUsersValidation _validation;
    private readonly IUserSession _session;


    public UserAdministratorController(ILogger<UserAdministratorController> logger, IUserAdministratorServices administratorServices, IUsersValidation validation, IUserSession session)
    {
        _logger = logger;
        _administratorServices = administratorServices;
        _validation = validation;
        _session = session;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<UserAdministrator> users = await _administratorServices.AdministratorsList();
            return View(users);
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

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(UserAdministrator user, string confirmPass)
    {
        try
        {

            if (string.IsNullOrEmpty(confirmPass))
            {
                TempData["ErrorPass"] = FeedbackMessages.ConfirmPassword;
                return View(user);
            }

            if (ModelState.IsValid)
            {
                List<(string FieldName, string Message)> duplicateErrors = await _validation.CheckForDuplicateUserFields(user);

                foreach (var (FieldName, Message) in duplicateErrors)
                    ModelState.AddModelError(FieldName, Message);

                if (ModelState.ErrorCount > 0)
                    return View(user);

                if (!_validation.ValidatePassword(user.Password, confirmPass, this))
                    return View(user);

                /* Assim que todos os dados forem validados de acordo com as exigências; */

                TempData["SuccessMessage"] = FeedbackMessages.SuccessAdministratorRegister;

                await _administratorServices.RegisterAdministrator(user);
                return RedirectToAction("Index");
            }

            TempData.Clear();
            return View(user);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorAdministratorRegister} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[ADMINISTRATOR_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            user = null;
            return View(user);
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorAdministratorRegister + " : " + FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            user = null;
            return View(user);
        }
    }

    public IActionResult Edit(int id) => View(_administratorServices.SearchForId(id));

    [HttpPost]
    public async Task<IActionResult> Edit(ModifyUser modifyUser)
    {
        try
        {
            UserAdministrator userEdit = _administratorServices.SearchForId(modifyUser.Id);

            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();

                bool enableAccount = Request.Form["enableDisableAccount"] == "on";

                if (enableAccount)
                {
                    userEdit.UserStats = userEdit.UserStats == UserStatsEnum.Active ? UserStatsEnum.Inactive : UserStatsEnum.Active;
                }

                if (enableAccount == true && userInSession.Masp == modifyUser.Masp)
                {
                    TempData["ErrorMessage"] = "Você desabilitou sua conta. Não será possível realizar login novamente com as suas credenciais. Entre em contato com um administrador!";
                    
                    _session.UserCheckOut();
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

                userEdit = new UserAdministrator()
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

                TempData["SuccessMessage"] = FeedbackMessages.SuccessAdministratorEdit;

                await _administratorServices.AdministratorsEdit(userEdit);
                return RedirectToAction("Index");
            }

            return View(userEdit);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorAdministratorEdit} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[ADMINISTRATOR_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorAdministratorEdit;

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, UserAdministrator user)
    {
        try
        {
            if (decision == "delete")
            {
                if (user.Id != 0)
                {
                    TempData["SuccessMessage"] = FeedbackMessages.SuccessAdministratorDelete;

                    _administratorServices.DeleteAdministrator(user.Id);
                    return RedirectToAction("Index");
                }
            }

            throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorAdministratorDelete} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[ADMINISTRATOR_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorAdministratorDelete; // Mensagem de vizualização para o usuário;

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[ADMINISTRATOR_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
    }
}