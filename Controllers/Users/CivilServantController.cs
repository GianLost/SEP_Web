using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Helper.Validation;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Models.UsersModels;
using SEP_Web.Models.AssessmentsModels;
using SEP_Web.Interfaces.UsersInterfaces;
using SEP_Web.Interfaces.AssessmentsInterfaces;
using SEP_Web.ViewModels;
using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Controllers.UsersControllers;

[ServiceFilter(typeof(UserEvaluatorFilter))]
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

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<UserAdministrator> do serviço
            var query = _civilServantServices.ServantsAsQueryable();

            // Converter IQueryable<UserAdministrator> para IQueryable<UsersViewModel>
            var viewModels = ConvertToViewModel(query);

            // Aplicando a ordenação
            if (request.Order != null && request.Order.Any())
            {
                var columnIndex = request.Order[0].Column; // Índice da coluna que está sendo ordenada
                var sortDirection = request.Order[0].Dir; // Direção da ordenação: "asc" ou "desc"

                switch (columnIndex)
                {
                    case 0:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.Masp).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Masp).AsQueryable();
                        break;
                    case 1:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.UserStats).AsQueryable()  // Ajustar para a coluna correta
                            : viewModels.OrderByDescending(u => u.UserStats).AsQueryable();
                        break;
                    case 2:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.Name).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Name).AsQueryable();
                        break;
                    case 3:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.Login).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Login).AsQueryable();
                        break;
                    case 4:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.Email).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Email).AsQueryable();
                        break;
                    case 5:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.Phone).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Phone).AsQueryable();
                        break;
                    default:
                        break;
                }
            }

            // Filtragem para busca via search
            if (!string.IsNullOrEmpty(request.Search.Value))
            {
                string searchValue = request.Search.Value.ToLower();
                viewModels = viewModels.Where(vm =>
                    vm.Name.ToLower().Contains(searchValue) ||
                    vm.Login.ToLower().Contains(searchValue) ||
                    vm.Email.ToLower().Contains(searchValue) ||
                    vm.Phone.ToLower().Contains(searchValue)
                ).AsQueryable();
            }

            // Filtragem, paginação e ordenação pelo DataTables
            var filteredData = viewModels.Skip(request.Start).Take(request.Length).ToList();
            var response = new
            {
                draw = request.Draw,
                recordsTotal = viewModels.Count(),
                recordsFiltered = viewModels.Count(),
                data = filteredData
            };

            return Json(response);
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
    public async Task<IActionResult> Register(CivilServant servant, Assessment assessment, string confirmPass)
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
                await _assessmentServices.RegisterAssessments(assessment, servant);

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

    private static IQueryable<UsersViewModel> ConvertToViewModel(IQueryable<CivilServant> servants)
    {
        return servants.Select(x => new UsersViewModel
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
        var servant = await _civilServantServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", servant);
    }
}