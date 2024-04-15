using SEP_Web.Models.UsersModels;
using SEP_Web.Models.AssessmentsModels;

namespace SEP_Web.Services;

public interface IAssessmentServices
{
    Task<Assessment> RegisterAssessments(Assessment assessment, CivilServant servant);
    Task<Assessment> Assess(Assessment assess);
    Task<ICollection<Assessment>> AssessmentsList();
    Task<ICollection<Assessment>> AssessmentsList(int userEvaluatorId);
    Task<string> ServantName(int? Id);
    int? ServantMasp(int Id);
    Task<bool> IsUnderLicense(int civilServantId);
    Assessment SearchForId(int id);
}