using SEP_Web.Models;

namespace SEP_Web.Services;
public interface IAssessmentServices
{
    Task<Assessment> RegisterAssessments(Assessment assessment);
    Task<Assessment> Assess(Assessment assess);
    Task<ICollection<Assessment>> AssessmentsList();
    Task<ICollection<Assessment>> AssessmentsList(int id);
    Task<string> ServantName(int? Id);
    Assessment SearchForId(int id);
}