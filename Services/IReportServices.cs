using FastReport;

namespace SEP_Web.Services;
public interface IReportServices
{
    Task<string> GenerateReportFile(string webRootPath);
    Task<byte[]> PrintReportToPDF(int id);
    Task<byte[]> ExportToPDF(Report report);
}
