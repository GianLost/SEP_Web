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

namespace SEP_Web.Controllers.StructuresController;

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

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<Division> do serviço
            var query = _divisionServices.DivisionsAsQueryable();

            // Converter IQueryable<Division> para IQueryable<DivisionViewModel>
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
                            ? viewModels.OrderBy(u => u.Name).AsQueryable()
                            : viewModels.OrderByDescending(u => u.Name).AsQueryable();
                        break;
                    case 1:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.InstituitionName).AsQueryable()
                            : viewModels.OrderByDescending(u => u.InstituitionId).AsQueryable();
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
                    vm.InstituitionName.ToLower().Contains(searchValue)
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
        catch (MySqlException dbException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);

            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<InstituitionViewModel>()
            });
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);

            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<InstituitionViewModel>()
            });
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);

            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<InstituitionViewModel>()
            });
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