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
using SEP_Web.Services;
using SEP_Web.ViewModels;

namespace SEP_Web.Controllers;

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

    public async Task<IActionResult> Index()
    {
        try
        {
            bool userIsEvaluator = _httpContext.HttpContext.Session.GetInt32("userType") == Convert.ToInt32(UsersTypeEnum.User_Evaluator);
            ICollection<AssessmentViewModel> viewModels;

            if (userIsEvaluator)
            {
                int id = Convert.ToInt32(_httpContext.HttpContext.Session.GetInt32("userId"));
                ICollection<Assessment> evaluator = await _assessmentServices.AssessmentsList(id);

                viewModels = evaluator.Where(assessment => !(_assessmentServices.IsUnderLicense(assessment.CivilServantId).Result && assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED)).Select(assessment => new AssessmentViewModel(assessment, _httpContext, _assessmentServices)).ToList();
            }
            else
            {
                ICollection<Assessment> users = await _assessmentServices.AssessmentsList();

                viewModels = users.Where(assessment => !(_assessmentServices.IsUnderLicense(assessment.CivilServantId).Result && assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED)).Select(assessment => new AssessmentViewModel(assessment, _httpContext, _assessmentServices)).ToList();
            }

            return View(viewModels);
        }
        catch (MySqlException ex)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorServantList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<AssessmentViewModel>());
        }
        catch (ArgumentNullException ex2)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<AssessmentViewModel>());
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
