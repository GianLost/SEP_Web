using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Filters;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.StructuresInterfaces;
using MySqlConnector;
using SEP_Web.Helper.Messages;
using System.Reflection;
using SEP_Web.ViewModels;
using SEP_Web.Models.DataTableModels;
using SEP_Web.Interfaces.DataTableInterfaces;

namespace SEP_Web.Controllers.StructuresController;

[ServiceFilter(typeof(UserAdminFilter))]
public class InstituitionController : Controller
{
    private readonly ILogger<InstituitionController> _logger;
    private readonly IInstituitionServices _instituitionServices;
    private readonly IDataTableService _dataTableService;
    private readonly IUserSession _session;

    public InstituitionController(ILogger<InstituitionController> logger, IInstituitionServices instituitionServices, IDataTableService dataTableService, IUserSession session)
    {
        _logger = logger;
        _instituitionServices = instituitionServices;
        _dataTableService = dataTableService;
        _session = session;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<Instituition> do serviço
            var query = _instituitionServices.SectionsAsQueryable();

            // Converter IQueryable<Instituition> para IQueryable<InstituitionViewModel>
            var instituitionViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                instituitionViewModelQuery = instituitionViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                instituitionViewModelQuery,
                request
            );

            // Verificar se o retorno é nulo ou vazio
            if (response == null || response.Data.Count == 0)
            {
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);
            }

            // Retornar os dados no formato JSON esperado pelo DataTables
            return Json(new
            {
                draw = response.Draw,
                recordsTotal = response.RecordsTotal,
                recordsFiltered = response.RecordsFiltered,
                data = response.Data
            });
        }
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorInstituitionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<InstituitionViewModel>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<InstituitionViewModel>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<InstituitionViewModel>());
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
            _logger.LogError("Não foi possível cadsatrar o órgão. Error : {Message}", e.Message);
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
            _logger.LogError("Não foi possível editar o órgão. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível editar o órgão!" });
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
            _logger.LogError("Não foi possível excluir o órgão. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }

    }

    private static IQueryable<InstituitionViewModel> ConvertToViewModel(IQueryable<Instituition> instituitions)
    {
        return instituitions.Select(x => new InstituitionViewModel
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var section = await _instituitionServices.GetByIdAsync(id);
        return PartialView("_EditModal", section);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var section = await _instituitionServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", section);
    }
}
