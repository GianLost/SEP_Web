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
public class DivisionController : Controller
{
    private readonly ILogger<DivisionController> _logger;
    private readonly IDivisionServices _divisionServices;
    private readonly IDataTableService _dataTableService;
    private readonly IUserSession _session;

    public DivisionController(ILogger<DivisionController> logger, IDivisionServices divisionServices, IDataTableService dataTableService, IUserSession session)
    {
        _logger = logger;
        _divisionServices = divisionServices;
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
            // Obter IQueryable<Division> do serviço
            var query = _divisionServices.SectionsAsQueryable();

            // Converter IQueryable<Division> para IQueryable<DivisionViewModel>
            var divisionViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                divisionViewModelQuery = divisionViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                divisionViewModelQuery,
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

    private static IQueryable<DivisionViewModel> ConvertToViewModel(IQueryable<Division> divisions)
    {
        return divisions.Select(x => new DivisionViewModel
        {
            Id = x.Id,
            Name = x.Name,
            InstituitionName = x.Instituition.Name,
            InstituitionId = x.InstituitionId
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var division = await _divisionServices.GetByIdAsync(id);
        return PartialView("_EditModal", division);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var division = await _divisionServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", division);
    }
}