using FastReport;
using FastReport.Export.PdfSimple;
using SEP_Web.Models;

namespace SEP_Web.Services;

public class ReportServices : IReportServices
{
    private readonly ILogger<ReportServices> _logger;
    private readonly IUserEvaluatorServices _evaluatorServices;
    private readonly ICivilServantServices _servantServices;
    private readonly IAssessmentServices _assessmentServices;
    private readonly IWebHostEnvironment _environment;

    public ReportServices(ILogger<ReportServices> logger, IUserEvaluatorServices evaluatorServices, ICivilServantServices servantServices, IAssessmentServices assessmentServices, IWebHostEnvironment environment)
    {
        _logger = logger;
        _evaluatorServices = evaluatorServices ?? throw new ArgumentNullException(nameof(evaluatorServices));
        _servantServices = servantServices ?? throw new ArgumentNullException(nameof(servantServices));
        _assessmentServices = assessmentServices ?? throw new ArgumentNullException(nameof(assessmentServices));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task<string> GenerateReportFile(string webRootPath)
    {
        try
        {
           string directoryPath = Path.Combine(webRootPath, @"Docs");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string reportFileName = $"Assessment_Report.frx";
            string reportFilePath = Path.Combine(directoryPath, reportFileName);

            using (Report r = new())
            {
                ICollection<CivilServant> servantList = await _servantServices.ServantsList();
                ICollection<Assessment> assessment = await _assessmentServices.AssessmentsList();
                ICollection<UserEvaluator> evaluatorList = await _evaluatorServices.EvaluatorsList();

                r.Report.Dictionary.RegisterBusinessObject(evaluatorList, "evaluatorList", 10, true);
                r.Report.Dictionary.RegisterBusinessObject(servantList, "servantList", 10, true);
                r.Report.Dictionary.RegisterBusinessObject(assessment, "assessment", 10, true);

                r.Report.Save(reportFilePath);
            }

            return reportFilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar o arquivo de relatório .frx.", ex);
            return null;
        }
    }

    public async Task<byte[]> PrintReportToPDF(int id)
    {
        try
        {
            string reportFile = Path.Combine(_environment.WebRootPath, @"Docs\Assessment_Report.frx");

            using Report r = new();

            ICollection<CivilServant> servantsList = await _servantServices.ServantsList(id);
            ICollection<Assessment> assessments = await _assessmentServices.AssessmentsList(id);
            ICollection<UserEvaluator> evaluatorsList = await _evaluatorServices.EvaluatorsList(id);

            r.Report.Load(reportFile);

            r.Report.Dictionary.RegisterBusinessObject(evaluatorsList, "evaluatorList", 10, true);
            r.Report.Dictionary.RegisterBusinessObject(servantsList, "servantList", 10, true);
            r.Report.Dictionary.RegisterBusinessObject(assessments, "assessment", 10, true);

            r.Prepare();

            return await ExportToPDF(r);

        }
        catch (Exception e)
        {
            _logger.LogError("Erro ao tentar imprimir o boletim em PDF!", e.Message);
            return null;
        }
    }

    public async Task<byte[]> ExportToPDF(Report report)
    {
        using MemoryStream ms = new();
        
        new PDFSimpleExport().Export(report, ms);
        await ms.FlushAsync();
        return ms.ToArray();
    }

}
