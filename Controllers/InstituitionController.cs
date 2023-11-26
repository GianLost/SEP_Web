using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Filters;
using SEP_Web.Models;
using SEP_Web.Services;
using MySqlConnector;
using SEP_Web.Helper.Messages;

namespace SEP_Web.Controllers;

[UserAdminFilter]
public class InstituitionController : Controller
{
    private readonly ILogger<InstituitionController> _logger;
    private readonly IInstituitionServices _instituitionServices;
    private readonly IUserSession _session;

    public InstituitionController(ILogger<InstituitionController> logger, IInstituitionServices instituitionServices, IUserSession session)
    {
        _logger = logger;
        _instituitionServices = instituitionServices;
        _session = session;
    }

    public async Task<IActionResult> Index()
    {
       try
       {
            ICollection<Instituition> instituitions = await _instituitionServices.InstituitionsList();
            return View(instituitions);
       }
       catch (MySqlException dbException)
       {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorInstituitionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<Instituition>());
       }
       catch (ArgumentNullException ex)
       {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<Instituition>());
       }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Instituition instituition)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                instituition.UserAdministratorId = userInSession.Id;

                await _instituitionServices.RegisterInstituition(instituition);
                TempData["SuccessMessage"] = "Órgão cadastrado com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadsatrar o órgão!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar o órgão", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar o órgão!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Instituition instituition)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                
                instituition.UserAdministratorId = userInSession.Id;
                instituition.LastModifiedBy = userInSession.Login;

                await _instituitionServices.InstituitionEdit(instituition);
                TempData["SuccessMessage"] = "órgão editado com sucesso.";
                return Json(new { stats = "OK" });
            }
            
            return Json(new { stats = "ERROR" });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar o órgão.";
            _logger.LogError("Não foi possível editar o órgão", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar o órgão!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, Instituition instituition)
    {

        try
        {
            if (decision == "delete")
            {
                if (instituition.Id != 0)
                {
                    _instituitionServices.DeleteInstituition(instituition.Id);
                    TempData["SuccessMessage"] = "Órgão excluído com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir o órgão.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir o órgão.";
            _logger.LogError("Não foi possível excluir o órgão", e.Message);
            return RedirectToAction("Index");
        }

    }

}
