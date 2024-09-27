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
using System.Linq.Dynamic.Core;

namespace SEP_Web.Controllers.StructuresController;

[ServiceFilter(typeof(UserAdminFilter))]
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

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(DataTableRequest request)
    {
        try
        {
            // Obter IQueryable<Section> do serviço
            var query = _sectionServices.SectionsAsQueryable();

            // Converter IQueryable<Section> para IQueryable<SectionViewModel>
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
                            ? viewModels.OrderBy(u => u.DivisionName).AsQueryable()
                            : viewModels.OrderByDescending(u => u.DivisionName).AsQueryable();
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
                    vm.DivisionName.ToLower().Contains(searchValue)
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
            // Log de erro para exceções MySQL
            _logger.LogError("{exceptionMessage} : {Message}, ErrorCode = {errorCode} - Represents {Error} ",
                ExceptionMessages.ErrorDatabaseConnection, dbException.Message.ToUpper(), dbException.Number, dbException.ErrorCode);
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorSectionList} {ExceptionMessages.ErrorDatabaseConnection}"; // Mensagem para o usuário;

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
            // Log de erro para exceções ArgumentNull
            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'",
                ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);

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
            // Log de erro para exceções TargetParameterCount
            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'",
                FeedbackMessages.ErrorEmptyCollection, ex2.Message, ex2.InnerException);

            return new JsonResult(new
            {
                draw = 0,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<InstituitionViewModel>()
            });
        }
        catch (Exception ex)
        {
            // Log de erro genérico
            _logger.LogError("Erro inesperado: {Message}", ex.Message);
            return new JsonResult(new
            {
                draw = request.Draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new List<InstituitionViewModel>()
            });
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