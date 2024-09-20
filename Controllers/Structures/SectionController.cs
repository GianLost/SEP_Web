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
using System.Linq.Dynamic.Core;

namespace SEP_Web.Controllers.StructuresController;

[ServiceFilter(typeof(UserAdminFilter))]
public class SectionController : Controller
{
    private readonly ILogger<SectionController> _logger;
    private readonly ISectionServices _sectionServices;
    private readonly IDataTableService _dataTableService;
    private readonly IUserSession _session;

    public SectionController(ILogger<SectionController> logger, ISectionServices sectionServices, IDataTableService dataTableService, IUserSession session)
    {
        _logger = logger;
        _sectionServices = sectionServices;
        _dataTableService = dataTableService;
        _session = session;
    }

    public IActionResult Index()
    {
        return View(); // A view não precisa receber a lista de sections.
    }

    [HttpPost]
    public async Task<IActionResult> Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<Section> do serviço
            var query = _sectionServices.SectionsAsQueryable();

            // Converter IQueryable<Section> para IQueryable<SectionViewModel>
            var sectionViewModelQuery = ConvertToViewModel(query);

            // Aplicar filtros de pesquisa, se houver
            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                var searchValue = request.Search.Value.ToLower();
                sectionViewModelQuery = sectionViewModelQuery.Where(s => s.Name.ToLower().Contains(searchValue));
            }

            // Obter dados paginados, filtrados e ordenados
            var response = await _dataTableService.GetPaginatedResponseAsync(
                sectionViewModelQuery,
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
            // Log de erro para exceções MySQL
            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ",
                ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorSectionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem para o usuário;

            return Json(new { error = FeedbackMessages.ErrorSectionList });
        }
        catch (ArgumentNullException ex)
        {
            // Log de erro para exceções ArgumentNull
            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'",
                ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            TempData["ErrorMessage"] = ExceptionMessages.ErrorArgumentNullException; // Mensagem para o usuário;

            return Json(new { error = ExceptionMessages.ErrorArgumentNullException });
        }
        catch (TargetParameterCountException ex2)
        {
            // Log de erro para exceções TargetParameterCount
            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'",
                FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);
            TempData["ErrorMessage"] = FeedbackMessages.ErrorEmptyCollection; // Mensagem para o usuário;

            return Json(new { error = FeedbackMessages.ErrorEmptyCollection });
        }
        catch (Exception ex)
        {
            // Log de erro genérico
            _logger.LogError("Erro inesperado: {Message}", ex.Message);
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado."; // Mensagem para o usuário;

            return Json(new { error = "Ocorreu um erro inesperado." });
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
            _logger.LogError("Não foi possível cadsatrar a seção. Error : {Message}", e.Message);
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
            _logger.LogError("Não foi possível editar a seção. Error : {Message}", e.Message);
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
            _logger.LogError("Não foi possível excluir a seção. Error : {Message}", e.Message);
            return RedirectToAction("Index");
        }
    }

    private static IQueryable<SectionViewModel> ConvertToViewModel(IQueryable<Section> sections)
    {
        return sections.Select(x => new SectionViewModel
        {
            Id = x.Id,
            Name = x.Name,
            DivisionName = x.Division.Name,
            DivisionId = x.DivisionId
        });
    }

    public async Task<IActionResult> EditModal(int id)
    {
        var section = await _sectionServices.GetByIdAsync(id);
        return PartialView("_EditModal", section);
    }

    public async Task<IActionResult> DeleteModal(int id)
    {
        var section = await _sectionServices.GetByIdAsync(id);
        return PartialView("_DeleteModal", section);
    }
}