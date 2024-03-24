using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Filters;
using SEP_Web.Models;
using SEP_Web.Services;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using System.Reflection;
using SEP_Web.ViewModels;

namespace SEP_Web.Controllers;

[ServiceFilter(typeof(UserAdminFilter))]
public class DivisionController : Controller
{
    private readonly ILogger<DivisionController> _logger;
    private readonly IDivisionServices _divisionServices;
    private readonly IUserSession _session;

    public DivisionController(ILogger<DivisionController> logger, IDivisionServices divisionServices, IUserSession session)
    {
        _logger = logger;
        _divisionServices = divisionServices;
        _session = session;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ICollection<DivisionViewModel> divisions = await _divisionServices.DivisionsList();

            if (divisions == null)
                throw new ArgumentNullException(nameof(divisions), ExceptionMessages.ErrorArgumentNullException);

            if (divisions.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            return View(divisions ?? new List<DivisionViewModel>());
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorDivisionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<DivisionViewModel>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<DivisionViewModel>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<DivisionViewModel>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Division division)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                division.UserAdministratorId = userInSession.Id;

                await _divisionServices.RegisterDivision(division);
                TempData["SuccessMessage"] = "Divisão cadastrada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadsatrar a divisão!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar a divisão. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar a divisão!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Division division)
    {
        try
        {

            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();

                division.UserAdministratorId = userInSession.Id;
                division.LastModifiedBy = userInSession.Login;

                await _divisionServices.DivisionEdit(division);
                TempData["SuccessMessage"] = "Divisão editada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", divisionI = division.InstituitionId, divisionName = division.Name });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar a divisão.";
            _logger.LogError("Não foi possível editar a divisão. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar o órgão!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, Division division)
    {

        try
        {
            if (decision == "delete")
            {
                if (division.Id != 0)
                {
                    _divisionServices.DeleteDivision(division.Id);
                    TempData["SuccessMessage"] = "Divisão excluída com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir a divisão.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir a divisão.";
            _logger.LogError("Não foi possível excluir a divisão. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }
    }
}