using FastReport;
using SEP_Web.Models.UsersModels;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.AssessmentsModels;

namespace SEP_Web.Interfaces.ReportsInterfaces;

public interface IReportServices
{
    Task<string> GenerateReportFile(string webRootPath);
    Task<byte[]> PrintReportToPDF(int id);
    Task<byte[]> ExportToPDF(Report report);
    Task<ICollection<CivilServant>> ServantsListToGenerateReport(int id);
    Task<ICollection<Assessment>> AssessmentsListToGenerateReport(int id);
    Task<ICollection<UserEvaluator>> EvaluatorsListToGenerateReport(int id);
    Task<ICollection<Instituition>> InstituitionsListToGenerateReport(int id);
}