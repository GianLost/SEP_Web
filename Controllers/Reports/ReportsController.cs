using Microsoft.AspNetCore.Mvc;
using SEP_Web.Filters;
using SEP_Web.Interfaces.ReportsInterfaces;

namespace SEP_Web.Controllers.ReportsController;

[ServiceFilter(typeof(UserEvaluatorFilter))]
public class ReportsController : Controller
{
    private readonly ILogger<ReportsController> _logger;
    private readonly IReportServices _reportServices;
    private readonly IWebHostEnvironment _environment;

    public ReportsController(ILogger<ReportsController> logger, IReportServices reportServices, IWebHostEnvironment environment)
    {
        _logger = logger;
        _reportServices = reportServices;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> CreateAssessmentReport()
    {
        string webRootPath = _environment.WebRootPath ?? throw new InvalidOperationException("WebRootPath não pode ser nulo.");

        string reportFilePath = await _reportServices.GenerateReportFile(webRootPath);

        if (reportFilePath != null)
            return Ok(reportFilePath);
        else
            return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> PrintAssessmentsToPDF(int id)
    {
        byte[] pdfBytes = await _reportServices.PrintReportToPDF(id);

        if (pdfBytes != null)
            return File(pdfBytes, "application/pdf");
        else
            return RedirectToAction("Index", "Home");
    }

}
