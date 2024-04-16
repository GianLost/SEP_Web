using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.UsersModels;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.AssessmentsModels;
using SEP_Web.Interfaces.AssessmentsInterfaces;
using SEP_Web.Interfaces.ReportsInterfaces;

namespace SEP_Web.Services.ReportsRepository;

public class ReportServices : IReportServices
{
    private readonly ILogger<ReportServices> _logger;
    private readonly SEP_WebContext _database;
    private readonly IAssessmentServices _assessmentServices;
    private readonly IWebHostEnvironment _environment;

    public ReportServices(ILogger<ReportServices> logger, SEP_WebContext database, IAssessmentServices assessmentServices, IWebHostEnvironment environment)
    {
        _logger = logger;
        _database = database;
        _assessmentServices = assessmentServices ?? throw new ArgumentNullException(nameof(assessmentServices));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task<string> GenerateReportFile(string webRootPath)
    {
        try
        {
           string directoryPath = Path.Combine(webRootPath, @"Reports");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string reportFileName = $"Boletim.frx";
            string reportFilePath = Path.Combine(directoryPath, reportFileName);

            using(Report r = new())
            {
                ICollection<Assessment> assessment = await _assessmentServices.AssessmentsList();

                r.Report.Dictionary.RegisterBusinessObject(assessment, "assessment", 10, true);

                r.Report.Save(reportFilePath);
            }

            return reportFilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar o arquivo de relatório .frx. Error {Message}", ex);
            return null;
        }
    }

    public async Task<byte[]> PrintReportToPDF(int id)
    {
        try
        {
            string reportFile = Path.Combine(_environment.WebRootPath, @"Reports/Boletim.frx");

            using Report r = new();

            ICollection<CivilServant> servantsList = await ServantsListToGenerateReport(id);
            ICollection<Assessment> assessments = await AssessmentsListToGenerateReport(id);
            ICollection<Instituition> instituitionsList = await InstituitionsListToGenerateReport(id);
            ICollection<UserEvaluator> evaluatorsList = await EvaluatorsListToGenerateReport(id);

            r.Report.Load(reportFile);

            r.Report.Dictionary.RegisterBusinessObject(evaluatorsList, "evaluatorList", 10, true);
            r.Report.Dictionary.RegisterBusinessObject(servantsList, "servantList", 10, true);
            r.Report.Dictionary.RegisterBusinessObject(assessments, "assessment", 10, true);
            r.Report.Dictionary.RegisterBusinessObject(instituitionsList, "instituitionsList", 10, true);

            r.Prepare();

            return await ExportToPDF(r);

        }
        catch (Exception e)
        {
            _logger.LogError("Erro ao tentar imprimir o boletim em PDF! Error {Message}", e.Message);
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

    public async Task<ICollection<CivilServant>> ServantsListToGenerateReport(int id)
    {
        try
        {

            ICollection<CivilServant> servants = await _database.Servants.Where(x => x.Id == id).ToListAsync();

            if (servants?.Count == 0)
                throw new ArgumentNullException(nameof(servants), ExceptionMessages.ErrorArgumentNullException);

            return servants ?? new List<CivilServant>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[REPORT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[REPORT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<CivilServant>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[REPORT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[REPORT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<CivilServant>();
        }
    }

    public async Task<ICollection<Assessment>> AssessmentsListToGenerateReport(int id)
    {
        try
        {
            ICollection<Assessment> assessment = await _database.Assessments.Where(x => x.CivilServantId == id).ToListAsync();

            if (assessment?.Count == 0)
                throw new ArgumentNullException(nameof(assessment), ExceptionMessages.ErrorArgumentNullException);

            return assessment ?? new List<Assessment>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[REPORT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[REPORT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Assessment>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[REPORT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[REPORT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<Assessment>();
        }
    }

    public async Task<ICollection<UserEvaluator>> EvaluatorsListToGenerateReport(int id)
    {
        try
        { 
            ICollection<CivilServant> servants = await _database.Servants.Where(x => x.Id == id).ToListAsync();

            int? assessmentEvaluatorId = servants.Select(x => x.UserEvaluatorId1).FirstOrDefault();
            
            ICollection<UserEvaluator> evaluatorList = await _database.Evaluators.Where(x => x.Id == assessmentEvaluatorId).ToListAsync();

            if (evaluatorList?.Count == 0)
                throw new ArgumentNullException(nameof(evaluatorList), ExceptionMessages.ErrorArgumentNullException);
            
            return evaluatorList ?? new List<UserEvaluator>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[REPORT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[REPORT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<UserEvaluator>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[REPORT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[REPORT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<UserEvaluator>();
        }
    }

    public async Task<ICollection<Instituition>> InstituitionsListToGenerateReport(int id)
    {
        try
        { 
            ICollection<CivilServant> servants = await _database.Servants.Where(x => x.Id == id).ToListAsync();

            int? assessmentInstituitionId = servants.Select(x => x.InstituitionId).FirstOrDefault();
            
            ICollection<Instituition> instituitionList = await _database.Instituitions.Where(x => x.Id == assessmentInstituitionId).ToListAsync();

            if (instituitionList?.Count == 0)
                throw new ArgumentNullException(nameof(instituitionList), ExceptionMessages.ErrorArgumentNullException);
            
            return instituitionList ?? new List<Instituition>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[REPORT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[REPORT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Instituition>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[REPORT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[REPORT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<Instituition>();
        }
    }

}