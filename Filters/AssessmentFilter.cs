using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;
public class AssessmentFilter : ActionFilterAttribute
{
    private readonly IFilterServices _filters;

    public AssessmentFilter(IFilterServices filters)
    {
        _filters = filters;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string url = context.HttpContext.Request.Path.Value;

        Users user = _filters.GetUserFromSession(context);

        if (user == null || user.UserStats != UserStatsEnum.Active)
            _filters.RedirectToLogin(context);

        if (url.Contains("ToAssess"))
        {
            int? assessmentId = GetAssessmentId(context);

            if (assessmentId.HasValue)
            {
                using SEP_WebContext _database = new();

                Assessment assessment = _database.Assessments.Find(assessmentId.Value);

                if (assessment != null)
                {
                    CivilServant servant = _database.Servants.FirstOrDefault(x => x.Id == assessment.CivilServantId);

                    if (servant.UserStats == UserStatsEnum.UnderLicense) // Impede que avaliações de servidores que estejam sob licença sejam acessadas em qualquer instância;
                    {
                        _filters.RedirectToAssessments(context);
                        return;
                    }

                    if (!DontAccessEvaluatedAssessment(user, assessment)) // Valida o acesso de usuários à avaliações já avaliadas;
                    {
                        _filters.RedirectToAssessments(context);
                        return;
                    }

                    if (!DontAccessAssessmentIsNotEvaluator(user, assessment)) // valida o acesso de usuários em avaliações permitindo que apenas o responsável pela avaliação possa acessá-la, exceto quando se tratar de um administrador, que terá o aceso irrestrito às avaliações desde que o servidor avaliado não esteja sob licença;
                    {
                        _filters.RedirectToAssessments(context);
                        return;
                    }
                    return;
                }
            }
        }

        base.OnActionExecuted(context);
    }

    private static int? GetAssessmentId(ActionExecutedContext context)
    {
        if (context.HttpContext.Request.RouteValues.TryGetValue("id", out var id))
            if (id is string idString && int.TryParse(idString, out var assessmentId))
                return assessmentId;

        return null;
    }

    private static bool DontAccessEvaluatedAssessment(Users user, Assessment assessment)
    {
        if (user.UserType == UsersTypeEnum.User_Admin && assessment.Stats == AssessmentStatsEnum.EVALUATED)
            return true; // Permite a um administrador acessar avaliações já avaliadas;

        if (user.UserType != UsersTypeEnum.User_Admin && assessment.Stats == AssessmentStatsEnum.EVALUATED)
            return false; // nega o acesso a avaliações já avaliadas à um usuário que não seja administrador;

        return true;
    }

    private static bool DontAccessAssessmentIsNotEvaluator(Users user, Assessment assessment)
    {
        if (user.UserType == UsersTypeEnum.User_Admin)
            return true; // Ao verificar se um usuário é administrador, permite que ele acesse qualquer avaliação, desde que o servidor a ser avaliado não esteja sob licença;

        return assessment.UserEvaluatorId1 == user.Id || assessment.UserEvaluatorId2 == user.Id; // Caso o usuário seja um avaliador ele poderá acesar somente avaliações em que ele conste como um dos avaliadores dos ervidor a ser avaliado;
    }
}