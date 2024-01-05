using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly ILogger<AssessmentsController> _logger;
        private readonly IAssessmentServices _assessmentServices;

        public AssessmentsController(ILogger<AssessmentsController> logger, IAssessmentServices assessmentServices)
        {
            _logger = logger;
            _assessmentServices = assessmentServices;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ICollection<Assessment> users = await _assessmentServices.AssessmentsList();
                return View(users);
            }
            catch (MySqlException ex)
            {
                // MYSQL EXEPTIONS :

                _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
                TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorServantList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

                return View(new List<Assessment>());
            }
            catch (ArgumentNullException ex2)
            {
                // NULL EXEPTIONS :

                _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
                TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

                return View(new List<Assessment>());
            }
        }

        public async Task<IActionResult> IndexForId(int id)
        {
            try
            {
                ICollection<Assessment> users = await _assessmentServices.AssessmentsList(id);
                return View(users);
            }
            catch (MySqlException ex)
            {
                // MYSQL EXEPTIONS :

                _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, ex.Message.ToUpper(), ex.Number, ex.ErrorCode);
                TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorServantList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

                return View(new List<Assessment>());
            }
            catch (ArgumentNullException ex2)
            {
                // NULL EXEPTIONS :

                _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
                TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

                return View(new List<Assessment>());
            }
        }

        public IActionResult ToAssess(int id) => View(_assessmentServices.SearchForId(id));

        [HttpPost]
        public async Task<IActionResult> ToAssess(Assessment assess)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string name = await _assessmentServices.ServantName(assess.CivilServantId);
                    TempData["SuccessMessage"] = $"O servidor {name}, foi avaliado na {assess.Phase}º Etapa com sucesso !";

                    await _assessmentServices.Assess(assess);
                    return RedirectToAction("Index");
                }

                TempData["ValidationMessage"] = $"* Todos os critérios devem ser avaliados.";

                return View(assess);
            }
            catch (MySqlException dbException)
            {
                // MYSQL EXEPTIONS :

                TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorEvaluatorEdit} {ExceptionMessages.ErrorDatabaseConnection}";

                _logger.LogError("[ASSESSMENTS_CONTROLLER]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

                _logger.LogError("[ASSESSMENTS_CONTROLLER] : Detalhamento dos erros: {Description} - ", dbException.StackTrace.Trim());

                assess = null;
                return View(assess);
            }
            catch (ArgumentNullException nullException)
            {
                // NULL EXEPTIONS :

                TempData["ErrorMessage"] = FeedbackMessages.ErrorServantEdit;

                _logger.LogWarning("[ASSESSMENTS_CONTROLLER]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

                _logger.LogWarning("[ASSESSMENTS_CONTROLLER]: {Description}", nullException.StackTrace.Trim());

                assess = null;
                return View(assess);
            }
        }
    }
}