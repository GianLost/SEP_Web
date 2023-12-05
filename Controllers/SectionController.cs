using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Filters;
using SEP_Web.Models;
using SEP_Web.Services;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using System.Reflection;

namespace SEP_Web.Controllers;

[UserAdminFilter]
public class SectionController : Controller
{
    private readonly ILogger<SectionController> _logger;
    private readonly ISectionServices _sectionServices;
    private readonly IUserSession _session;

    public SectionController(ILogger<SectionController> logger, ISectionServices sectionServices, IUserSession session)
    {
        _logger = logger;
        _sectionServices = sectionServices;
        _session = session;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<Section> sections = await _sectionServices.SectionsList();

            if (sections == null)
                throw new ArgumentNullException(nameof(sections), ExceptionMessages.ErrorArgumentNullException);

            if (sections?.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            return View(sections ?? new List<Section>());
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorSectionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<Section>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<Section>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<Section>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Section sections)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                sections.UserAdministratorId = userInSession.Id;

                await _sectionServices.RegisterSection(sections);
                TempData["SuccessMessage"] = "Seção cadastrada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadsatrar a seção!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar a seção", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar a seção!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Section section)
    {
        try
        {

            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                section.UserAdministratorId = userInSession.Id;
                section.LastModifiedBy = userInSession.Login;

                await _sectionServices.SectionEdit(section);
                TempData["SuccessMessage"] = "Seção editada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", sectionI = section.DivisionId, sectionName = section.Name });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar a seção.";
            _logger.LogError("Não foi possível editar a seção", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar o órgão!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, Section section)
    {

        try
        {
            if (decision == "delete")
            {
                if (section.Id != 0)
                {
                    _sectionServices.DeleteSection(section.Id);
                    TempData["SuccessMessage"] = "Seção excluída com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir a seção.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir a seção.";
            _logger.LogError("Não foi possível excluir a seção", e.Message);
            return RedirectToAction("Index");
        }
    }
}
