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
using SEP_Web.Interfaces.DataTableInterfaces;

namespace SEP_Web.Controllers.StructuresController;

[ServiceFilter(typeof(UserAdminFilter))]
public class SectorController : Controller
{
    private readonly ILogger<SectorController> _logger;
    private readonly ISectorServices _sectorServices;
    private readonly IDataTableService _dataTableService;
    private readonly IUserSession _session;

    public SectorController(ILogger<SectorController> logger, ISectorServices sectorServices, IDataTableService dataTableService, IUserSession session)
    {
        _logger = logger;
        _sectorServices = sectorServices;
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
            // Obter IQueryable<Section> do serviço
            var query = _sectorServices.SectorsAsQueryable();

            // Converter IQueryable<Section> para IQueryable<SectionViewModel>
            var sectorViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                sectorViewModelQuery = sectorViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                sectorViewModelQuery,
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
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorSectorList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem de vizualização para o usuário;

            return View(new List<SectorViewModel>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<SectorViewModel>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<SectorViewModel>());
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