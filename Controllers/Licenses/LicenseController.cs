using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.LicensesModels;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.LicensesInterfaces;
using SEP_Web.Interfaces.DataTableInterfaces;
using SEP_Web.Models.DataTableModels;
using SEP_Web.ViewModels;
using SEP_Web.Database;

namespace SEP_Web.Controllers.LicensesController;

[ServiceFilter(typeof(UserAdminFilter))]
public class LicenseController : Controller
{
    private readonly ILogger<LicenseController> _logger;
    private readonly IUserSession _session;
    private readonly SEP_WebContext _database;
    private readonly IDataTableService _dataTableService;
    private readonly ILicenseServices _licenses;

    public LicenseController(ILogger<LicenseController> logger, IUserSession session, SEP_WebContext database, IDataTableService dataTableService, ILicenseServices licenses)
    {
        _logger = logger;
        _session = session;
        _database = database;
        _dataTableService = dataTableService;
        _licenses = licenses;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<License> do serviço
            var query = _licenses.LicensesAsQueryable();

            // Converter IQueryable<Sector> para IQueryable<LicenseViewModel>
            var licenseViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                licenseViewModelQuery = licenseViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                licenseViewModelQuery,
                request
            );

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

            return View(new List<Licenses>());
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem de vizualização para o usuário;

            return View(new List<Licenses>());
        }
        catch (TargetParameterCountException ex2)
        {
            // EMPTY EXEPTIONS :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem de vizualização para o usuário;

            return View(new List<Licenses>());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Licenses license)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                license.UserAdministratorId = userInSession.Id;

                await _licenses.RegisterLicense(license);
                TempData["SuccessMessage"] = "Liceça cadastrada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", message = "Não foi possível cadastrar a licença!" });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível cadsatrar a licença. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível cadastrar a liceça!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Licenses license)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();

                license.UserAdministratorId = userInSession.Id;
                license.LastModifiedBy = userInSession.Login;

                await _licenses.LicensesEdit(license);
                TempData["SuccessMessage"] = "Licença editada com sucesso.";
                return Json(new { stats = "OK" });
            }

            return Json(new { stats = "ERROR", licenseName = license.Name, licenseTime = license.Time });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar a licença.";
            _logger.LogError("Não foi possível editar a licença. Error : {Message}", e.Message);
            return Json(new { stats = "INVALID", message = "Não foi possível editar a licença!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, Licenses license)
    {
        try
        {
            if (decision == "delete")
            {
                if (license.Id != 0)
                {
                    _licenses.DeleteLicenses(license.Id);
                    TempData["SuccessMessage"] = "Licença excluída com sucesso.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possível excluir a licença.";
            }

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível excluir a licença.";
            _logger.LogError("Não foi possível excluir a licença. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }
    }

    private IQueryable<LicenseViewModel> ConvertToViewModel(IQueryable<Licenses> sector)
    {
        return _database.Licenses.Select(x => new LicenseViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Time = x.Time
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var license = await _licenses.GetByIdAsync(id);
        return PartialView("_EditModal", license);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var license = await _licenses.GetByIdAsync(id);
        return PartialView("_DeleteModal", license);
    }
}