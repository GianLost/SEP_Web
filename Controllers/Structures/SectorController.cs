using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.StructuresInterfaces;
using SEP_Web.ViewModels;
using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Controllers.StructuresController;

[ServiceFilter(typeof(UserAdminFilter))]
public class SectorController : Controller
{
    private readonly ILogger<SectorController> _logger;
    private readonly ISectorServices _sectorServices;
    private readonly IUserSession _session;

    public SectorController(ILogger<SectorController> logger, ISectorServices sectorServices, IUserSession session)
    {
        _logger = logger;
        _sectorServices = sectorServices;
        _session = session;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<Sector> do serviço
            var query = _sectorServices.SectorsAsQueryable();

            // Converter IQueryable<Sector> para IQueryable<SectorViewModel>
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
                            ? viewModels.OrderBy(u => u.SectionName).AsQueryable()
                            : viewModels.OrderByDescending(u => u.SectionName).AsQueryable();
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
                    vm.SectionName.ToLower().Contains(searchValue)
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
    public async Task<IActionResult> Register(Sector sector)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                sector.UserAdministratorId = userInSession.Id;

                await _sectorServices.RegisterSector(sector);

                TempData["SuccessMessage"] = "Setor cadastrado com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadsatrar o setor!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar o setor. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadsatrar o setor!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Sector sector)
    {
        try
        {

            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                sector.UserAdministratorId = userInSession.Id;
                sector.LastModifiedBy = userInSession.Login;

                await _sectorServices.SectorEdit(sector);
                TempData["SuccessMessage"] = "Setor editado com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", sectorI = sector.SectionId, sectorName = sector.Name });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar o setor.";
            _logger.LogError("Não foi possível editar o setor. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível editar o setor!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, Sector sector)
    {

        try
        {
            if (decision == "delete")
            {
                if (sector.Id != 0)
                {
                    _sectorServices.DeleteSector(sector.Id);
                    TempData["SuccessMessage"] = "Setor excluído com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir o setor.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir o setor.";
            _logger.LogError("Não foi possível excluir o setor. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }
    }

    private static IQueryable<SectorViewModel> ConvertToViewModel(IQueryable<Sector> sector)
    {
        return sector.Select(x => new SectorViewModel
        {
            Id = x.Id,
            Name = x.Name,
            SectionName = x.Section.Name,
            SectionId = x.SectionId
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var sector = await _sectorServices.GetByIdAsync(id);
        return PartialView("_EditModal", sector);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var sector = await _sectorServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", sector);
    }
}