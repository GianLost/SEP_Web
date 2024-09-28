using System.Data;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models.UsersModels;
using SEP_Web.Models.AssessmentsModels;
using SEP_Web.Interfaces.AssessmentsInterfaces;
using SEP_Web.ViewModels;
using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Controllers.AssessmentsController;

[ServiceFilter(typeof(AssessmentFilter))]
public class AssessmentsController : Controller
{
    private readonly ILogger<AssessmentsController> _logger;
    private readonly IAssessmentServices _assessmentServices;
    private readonly IHttpContextAccessor _httpContext;

    public AssessmentsController(ILogger<AssessmentsController> logger, IAssessmentServices assessmentServices, IHttpContextAccessor httpContext)
    {
        _logger = logger;
        _assessmentServices = assessmentServices;
        _httpContext = httpContext;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            bool userIsEvaluator = _httpContext.HttpContext.Session.GetInt32("userType") == Convert.ToInt32(UsersTypeEnum.User_Evaluator);
            ICollection<AssessmentViewModel> viewModels;

            if (userIsEvaluator)
            {
                int id = Convert.ToInt32(_httpContext.HttpContext.Session.GetInt32("userId"));
                ICollection<Assessment> evaluator = _assessmentServices.AssessmentsList(id).Result;

                viewModels = evaluator
                    .Where(assessment => !(_assessmentServices.IsUnderLicense(assessment.CivilServantId).Result && assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED))
                    .Select(assessment => new AssessmentViewModel(assessment, _httpContext, _assessmentServices)).ToList();
            }
            else
            {
                ICollection<Assessment> users = _assessmentServices.AssessmentsList().Result;

                viewModels = users
                    .Where(assessment => !(_assessmentServices.IsUnderLicense(assessment.CivilServantId).Result && assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED))
                    .Select(assessment => new AssessmentViewModel(assessment, _httpContext, _assessmentServices)).ToList();
            }

            // Aqui estamos aplicando a ordenação
            if (request.Order != null && request.Order.Any())
            {
                var columnIndex = request.Order[0].Column; // Índice da coluna que está sendo ordenada
                var sortDirection = request.Order[0].Dir; // Direção da ordenação: "asc" ou "desc"

                switch (columnIndex)
                {
                    case 0:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.StatusTitle).ToList()
                            : viewModels.OrderByDescending(vm => vm.StatusTitle).ToList();
                        break;
                    case 1:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.Phase).ToList()
                            : viewModels.OrderByDescending(vm => vm.Phase).ToList();
                        break;
                    case 2:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.Masp).ToList()
                            : viewModels.OrderByDescending(vm => vm.Masp).ToList();
                        break;
                    case 3:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.ServantName).ToList()
                            : viewModels.OrderByDescending(vm => vm.ServantName).ToList();
                        break;
                    case 4:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.StartDate).ToList()
                            : viewModels.OrderByDescending(vm => vm.StartDate).ToList();
                        break;
                    case 5:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(vm => vm.EndDate).ToList()
                            : viewModels.OrderByDescending(vm => vm.EndDate).ToList();
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
                    vm.Phase.ToString().ToLower().Contains(searchValue) ||
                    vm.Masp.ToLower().Contains(searchValue) ||
                    vm.ServantName.ToLower().Contains(searchValue)
                ).ToList();
            }

            // Filtragem, paginação e ordenação pelo DataTables
            var filteredData = viewModels.Skip(request.Start).Take(request.Length).ToList();
            var response = new
            {
                draw = request.Draw,
                recordsTotal = viewModels.Count,
                recordsFiltered = viewModels.Count, // Modifique se aplicar filtros adicionais
                data = filteredData
            };

            return Json(response);
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
                data = new List<AssessmentViewModel>()
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
                data = new List<AssessmentViewModel>()
            });
        }
        catch (Exception ex3)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex3.Message, ex3.InnerException);
            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<AssessmentViewModel>()
            });
        }
    }

    public IActionResult ToAssess(int id) => View(_assessmentServices.SearchForId(id));

    [HttpPost]
    public async Task<IActionResult> ToAssess(Assessment assess)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["ValidationMessage"] = $"* Todos os critérios devem ser avaliados.";
                return View(assess);
            }

            string name = await _assessmentServices.ServantName(assess.CivilServantId);
            TempData["SuccessMessage"] = $"O(A) servidor(a) {name}, foi avaliado na {assess.Phase}º Etapa com sucesso !";

            if (assess.Stats == AssessmentStatsEnum.EVALUATED)
            {
                string userSession = _httpContext.HttpContext.Session.GetString("userCheckIn");
                Users user = await JsonSerializer.DeserializeAsync<Users>(new MemoryStream(Encoding.UTF8.GetBytes(userSession)));

                assess.ModifyDate = DateTime.Now;
                assess.LastModifiedBy = user.Login;

                await _assessmentServices.Assess(assess);
                return RedirectToAction("Index");
            }

            await _assessmentServices.Assess(assess);
            return RedirectToAction("Index");
        }
        catch (MySqlException dbException)
        {
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorEdit} {ExceptionMessages.ErrorDatabaseConnection}";
            _logger.LogError("[ASSESSMENTS_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            _logger.LogError("[ASSESSMENTS_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());
        }
        catch (ArgumentNullException nullException)
        {
            TempData["ErrorMessage"] = FeedbackMessages.ErrorServantEdit;
            _logger.LogWarning("[ASSESSMENTS_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[ASSESSMENTS_CONTROLLER]: {Description}", nullException.StackTrace.Trim());
        }

        return View(assess);
    }

}