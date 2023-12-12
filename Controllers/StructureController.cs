using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP_Web.Filters;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

[UserEvaluatorFilter]
public class StructureController : Controller
{
    private readonly ILogger<StructureController> _logger;
     private readonly IUserEvaluatorServices _evaluatorServices;
    private readonly IDivisionServices _divisionServices;
    private readonly ISectionServices _sectionServices;
    private readonly ISectorServices _sectorServices;

    public StructureController(ILogger<StructureController> logger, IUserEvaluatorServices evaluatorServices, IDivisionServices divisionServices, ISectorServices sectorServices, ISectionServices sectionServices)
    {
        _logger = logger;
        _evaluatorServices = evaluatorServices;
        _divisionServices = divisionServices;
        _sectionServices = sectionServices;
        _sectorServices = sectorServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetDivisionsByInstituition(int instituitionId)
    {
        ICollection<Division> divisions = await _divisionServices.GetDivisionsAsync(instituitionId);

        IEnumerable<SelectListItem> divisionList = divisions.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(divisionList);
    }

    [HttpGet]
    public async Task<IActionResult> GetSectionsByDivisions(int divisionId)
    {
        ICollection<Section> sections = await _sectionServices.GetSectionsAsync(divisionId);

        IEnumerable<SelectListItem> sectionList = sections.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(sectionList);
    }

    [HttpGet]
    public async Task<IActionResult> GetSectorsBySections(int sectionId)
    {
        ICollection<Sector> sectors = await _sectorServices.GetSectorsAsync(sectionId);

        IEnumerable<SelectListItem> sectorList = sectors.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });

        return Json(sectorList);
    }

    [HttpGet]
    public async Task<IActionResult> GetEvaluatorsForInstituitionId(int UserEvaluatorId1)
    {
        ICollection<UserEvaluator> evaluators = await _evaluatorServices.GetFirstEvaluatorForRelationToServantAsync(UserEvaluatorId1);

        var evaluatorsList = evaluators.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });
        return Json(evaluatorsList);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSecondEvaluator(int UserEvaluatorId2, int UserEvaluatorId1)
    {
        ICollection<UserEvaluator> evaluators = await _evaluatorServices.GetSecondEvaluatorForRelationToServantAsync(UserEvaluatorId2, UserEvaluatorId1);
        
        var evaluatorsList = evaluators.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
        });
        return Json(evaluatorsList);
    }

    public async Task<IActionResult> ModifyStructures(ModifyStructures modifyStructures)
    {
        try
        {

            if (ModelState.IsValid)
            {
                await _evaluatorServices.EditStructures(modifyStructures);
                TempData["SuccessMessage"] = "Estruturas editadas com sucesso !";
                return Json(new { stats = StatsAJAXEnum.OK });
            }

            return Json(new { stats = StatsAJAXEnum.ERROR});
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "Não foi possível editar as estruturas.";
            _logger.LogError("Não foi possível editar as estruturas", e.Message);
            return Json(new { stats = StatsAJAXEnum.INVALID, message = "Não foi possível editar as estruturas!" });
        }
    }
}
