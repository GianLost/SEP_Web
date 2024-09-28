using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SEP_Web.Filters;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models.LicensesModels;
using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.LicensesInterfaces;
using SEP_Web.Interfaces.AssessmentsInterfaces;
using SEP_Web.Models.DataTableModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Controllers.LicensesController;

[ServiceFilter(typeof(UserAdminFilter))]
public class ServantLicenseController : Controller
{
    private readonly ILogger<ServantLicenseController> _logger;
    private readonly ILicenseServices _licenses;
    private readonly IServantLicenseServices _servantLicenses;
    private readonly IAssessmentServices _assessmentServices;
    private readonly IUserSession _session;

    public ServantLicenseController(ILogger<ServantLicenseController> logger, ILicenseServices licenses, IServantLicenseServices servantLicenses, IAssessmentServices assessmentServices, IUserSession session)
    {
        _logger = logger;
        _licenses = licenses;
        _servantLicenses = servantLicenses;
        _assessmentServices = assessmentServices;
        _session = session;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<ServantLicense> do serviço
            var query = _servantLicenses.ServantLicensesAsQueryable();

            // Converter IQueryable<ServantLicense> para IQueryable<ServantLicenseViewModel>
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
                            ? viewModels.OrderBy(u => u.Masp).AsQueryable()  // Ajustar para a coluna correta
                            : viewModels.OrderByDescending(u => u.Masp).AsQueryable();
                        break;
                    case 1:
                        viewModels = sortDirection == "asc"
                           ? viewModels.OrderBy(u => u.Name).AsQueryable()
                           : viewModels.OrderByDescending(u => u.Name).AsQueryable();
                        break;
                    case 2:
                        viewModels = sortDirection == "asc"
                            ? viewModels.OrderBy(u => u.LicenseName).AsQueryable()
                            : viewModels.OrderByDescending(u => u.LicenseName).AsQueryable();
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
                    vm.LicenseName.ToLower().Contains(searchValue)
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
                data = new List<ServantLicenseViewModel>()
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
                data = new List<ServantLicenseViewModel>()
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
                data = new List<ServantLicenseViewModel>()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(ServantLicense servantLicense)
    {
        try
        {
            if (ModelState.IsValid)
            {
                int maxDuration = await _licenses.GetMaxLicenseDuration(servantLicense.LicensesId);

                // Verificar se a diferença entre as datas excede a duração máxima
                TimeSpan? duration = servantLicense.EndDate - servantLicense.StartDate;

                if (duration.Value.Days > maxDuration)
                    return Json(new { stats = StatsAJAXEnum.ERROR_TIME });

                if (servantLicense.EndDate <= servantLicense.StartDate)
                    return Json(new { stats = StatsAJAXEnum.INVALID_TIME });

                await _servantLicenses.RegisterServantLicense(servantLicense);

                TempData["SuccessMessage"] = $" A Licença para o(a) servidor(a) {await _assessmentServices.ServantName(servantLicense.CivilServantId)} foi cadastrada com sucesso !";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            TempData["ErrorMessage"] = "Erro ao adicionar licença para o servidor.";
            return Json(new { stats = StatsAJAXEnum.ERROR, message = "Não foi possível adicionar a licença!" });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError("Não foi possível adicionar a licença. Error : {Message}", e.Message);
            return Json(new { stats = StatsAJAXEnum.INVALID, message = "Não foi possível adicionar a liceça!" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ServantLicense servantLicense)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users userInSession = await _session.SearchUserSession();
                int maxDuration = await _licenses.GetMaxLicenseDuration(servantLicense.LicensesId);

                // Verificar se a diferença entre as datas excede a duração máxima
                TimeSpan? duration = servantLicense.EndDate - servantLicense.StartDate;

                if (duration.Value.Days > maxDuration)
                    return Json(new { stats = StatsAJAXEnum.ERROR_TIME });

                if (servantLicense.EndDate <= servantLicense.StartDate)
                    return Json(new { stats = StatsAJAXEnum.INVALID_TIME });

                servantLicense.LastModifiedBy = userInSession.Login;

                await _servantLicenses.ServantLicensesEdit(servantLicense);

                TempData["SuccessMessage"] = $" A Licença do(a) servidor(a) {await _assessmentServices.ServantName(servantLicense.CivilServantId)} foi editada com sucesso !";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            TempData["ErrorMessage"] = "Erro ao editar licença para o servidor.";
            return Json(new { stats = StatsAJAXEnum.ERROR, civilServantId = servantLicense.CivilServantId, licenseName = servantLicense.LicensesId, sDate = servantLicense.StartDate, eDate = servantLicense.EndDate });
        }
        catch (Exception e)
        {
            _logger.LogError("Não foi possível editar a licença. Error : {Message}", e.Message);
            TempData["ErrorMessage"] = "Erro interno ao editar a licença!";
            return Json(new { stats = StatsAJAXEnum.INVALID, message = "Não foi possível adicionar a liceça!" });
        }
    }

    [HttpPost]
    public IActionResult Delete(string decision, ServantLicense servantLicense)
    {

        try
        {
            if (decision == "delete")
            {
                if (servantLicense.Id != 0)
                {
                    _servantLicenses.DeleteServantLicenses(servantLicense.Id);
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

    [HttpGet]
    public IActionResult GetLicenseDuration(int id)
    {
        try
        {
            Licenses license = _licenses.SearchForId(id);

            if (license != null)
                return Ok(license.Time);

            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Não foi possível capturar a licença. Error : {Message}", ex.Message);
            return StatusCode(500);
        }
    }

    private static IQueryable<ServantLicenseViewModel> ConvertToViewModel(IQueryable<ServantLicense> servantLicenses)
    {
        return servantLicenses.Select(x => new ServantLicenseViewModel
        {
            Id = x.Id,
            Masp = x.CivilServant.Masp,
            Name = x.CivilServant.Name,
            CivilServant = x.CivilServant,
            License = x.License,
            LicenseName = x.License.Name,
            StartDate = x.StartDate.HasValue ? x.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty, // Alterado para o formato correto
            EndDate = x.EndDate.HasValue ? x.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var servantLicense = await _servantLicenses.GetByIdAsync(id);
        return PartialView("_EditModal", servantLicense);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var servantLicenses = await _servantLicenses.GetByIdAsync(id);
        return PartialView("_DeleteModal", servantLicenses);
    }

}