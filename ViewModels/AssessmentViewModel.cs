using SEP_Web.Keys;
using SEP_Web.Models.AssessmentsModels;
using SEP_Web.Services;

namespace SEP_Web.ViewModels;

public class AssessmentViewModel
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IAssessmentServices _assessmentServices;

    public AssessmentViewModel(Assessment assessment, IHttpContextAccessor httpContext, IAssessmentServices assessmentServices)
    {
        _httpContext = httpContext;
        _assessmentServices = assessmentServices;

        AssessmentId = assessment.Id;

        int userType = Convert.ToInt32(UsersTypeEnum.User_Admin);

        StatusTitle = _assessmentServices.IsUnderLicense(assessment.CivilServantId).Result ? "Servidor(a) sob licença" : assessment.Stats == AssessmentStatsEnum.EVALUATED ? "Servidor(a) avaliado(a)" : "Servidor(a) não avaliado(a)";
        StatusClass = _assessmentServices.IsUnderLicense(assessment.CivilServantId).Result ? "text-warning" : assessment.Stats == AssessmentStatsEnum.EVALUATED ? "text-success" : "text-danger";
        StatusIcon = "radio_button_checked";
        RowColor = _assessmentServices.IsUnderLicense(assessment.CivilServantId).Result ? "table-warning" : assessment.Stats == AssessmentStatsEnum.EVALUATED ? "table-success" : "table-danger";
        Phase = assessment.Phase;
        Masp = $"{assessment.CivilServant.Masp:D8}";
        ServantName = assessment.CivilServant?.Name;
        StartDate = Convert.ToDateTime(assessment.StartEvaluationPeriod).ToString("dd/MM/yyyy");
        EndDate = assessment.EndEvaluationPeriod.HasValue ? Convert.ToDateTime(assessment.EndEvaluationPeriod).ToString("dd/MM/yyyy") : "-";
        CanAssess = !_assessmentServices.IsUnderLicense(assessment.CivilServantId).Result && !assessment.Stats.Equals(AssessmentStatsEnum.EVALUATED) || (_httpContext.HttpContext.Session.GetInt32("userType") == userType && assessment.Stats.Equals(AssessmentStatsEnum.EVALUATED));
    }

    public int AssessmentId { get; set; }
    public string RowColor { get; set; }
    public string StatusTitle { get; set; }
    public string StatusClass { get; set; }
    public string StatusIcon { get; set; }
    public int Phase { get; set; }
    public string Masp { get; set; }
    public string ServantName { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public bool CanAssess { get; set; }

}
