using SEP_Web.Models;

namespace SEP_Web.Services;
public interface IAssessmentServices
{
    Task<Assessment> RegisterAssessments(Assessment assessment);
    Task<Assessment> Assess(Assessment assess);
    Task<ICollection<Assessment>> AssessmentsList();
    Task<string> ServantName(int? Id);
    int? ServantMasp(int Id);
    Task<bool> IsUnderLicense(int civilServantId);
    Assessment SearchForId(int id);
}