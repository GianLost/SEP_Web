using Microsoft.AspNetCore.Mvc;
using SEP_Web.Filters;
using SEP_Web.Models;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.UsersInterfaces;
using MySqlConnector;
using SEP_Web.Helper.Validation;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Helper.Authentication;
using SEP_Web.ViewModels;
using SEP_Web.Interfaces.DataTableInterfaces;
using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Controllers.UsersControllers;

[ServiceFilter(typeof(UserEvaluatorFilter))]
public class UserEvaluatorController : Controller
{
    private readonly ILogger<UserEvaluatorController> _logger;
    private readonly IUserEvaluatorServices _evaluatorServices;
        private readonly IDataTableService _dataTableService;
    private readonly IUsersValidation _validation;
    private readonly IUserSession _session;

    public UserEvaluatorController(ILogger<UserEvaluatorController> logger, IUserEvaluatorServices evaluatorServices, IDataTableService dataTableService, IUsersValidation validation, IUserSession session)
    {
        _logger = logger;
        _evaluatorServices = evaluatorServices;
        _dataTableService = dataTableService;
        _validation = validation;
        _session = session;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<UserEvaluator> do serviço
            var query = _evaluatorServices.EvaluatorsAsQueryable();

            // Converter IQueryable<UserEvaluator> para IQueryable<UsersViewModel>
            var evaluatorViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                evaluatorViewModelQuery = evaluatorViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                evaluatorViewModelQuery,
                request
            );

            // Retornar os dados no formato JSON esperado pelo DataTables
            return Json(new
            {
                draw = response.Draw,
                recordsTotal = response.RecordsTotal,
                recordsFiltered = response.RecordsFiltered,
                data = response.Data
            });
        }
        catch (MySqlException ex)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<UsersViewModel>()
            });
        }
        catch (ArgumentNullException ex2)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<UsersViewModel>()
            });
        }
    }

    public IActionResult Register() => View();

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
                List<(string FieldName, string Message)> duplicateErrors = await _validation.CheckForDuplicateUserFields(evaluator);

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

    public IActionResult Edit(int id) => View(_evaluatorServices.SearchForId(id));

    [HttpPost]
    public async Task<IActionResult> Edit(ModifyUser modifyUser)
    {
        try
        {
            UserEvaluator userEdit = _evaluatorServices.SearchForId(modifyUser.Id);

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

                userEdit = new UserEvaluator()
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

                TempData["SuccessMessage"] = FeedbackMessages.SuccessEvaluatorEdit;

                await _evaluatorServices.EvaluatorsEdit(userEdit);
                return RedirectToAction("Index");
            }

            return View(userEdit);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorEdit} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[EVALUATOR_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[EVALUATOR_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorEvaluatorEdit;

            _logger.LogWarning("[EVALUATOR_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[EVALUATOR_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            modifyUser = null;
            return View(modifyUser);
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, UserEvaluator user)
    {
        try
        {
            if (decision == "delete")
            {
                if (user.Id != 0)
                {
                    TempData["SuccessMessage"] = FeedbackMessages.SuccessEvaluatorDelete;

                    _evaluatorServices.DeleteEvaluator(user.Id);
                    return RedirectToAction("Index");
                }
            }

            throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorDelete} {ExceptionMessages.ErrorDatabaseConnection}";

            _logger.LogError("[EVALUATOR_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            _logger.LogError("[EVALUATOR_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXEPTIONS :

            TempData["ErrorMessage"] = FeedbackMessages.ErrorEvaluatorDelete; // Mensagem de vizualização para o usuário;

            _logger.LogWarning("[EVALUATOR_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[EVALUATOR_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

            return RedirectToAction("Index");
        }
    }

    
    private static IQueryable<UsersViewModel> ConvertToViewModel(IQueryable<UserEvaluator> evaluators)
    {
        return evaluators.Select(x => new UsersViewModel
        {
            Id = x.Id,
            UserStats = x.UserStats,
            Masp = x.Masp,
            Name = x.Name,
            Login = x.Login,
            Email = x.Email,
            Phone = x.Phone
        });
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var evaluator = await _evaluatorServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", evaluator);
    }
}
