using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;

public class UserAdminFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {

        string userSession = context.HttpContext.Session.GetString("userCheckIn");
        string url = context.HttpContext.Request.Path.Value;

        if (string.IsNullOrEmpty(userSession))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
        }
        else
        {
            Users user = JsonSerializer.Deserialize<Users>(userSession);

            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }

            if (user.UserStats != UserStatsEnum.Active)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
            }

            if (user.UserType != UsersTypeEnum.User_Admin && url.Contains("ToAssess") && IsEvaluationAlreadyEvaluated(context))
            {
                // Nega acesso a usuários que não são administradores de acessarem uma avaliação já avaliada.
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Assessments" }, {"action", "Index" } });
            }

            if(url.Contains("ToAssess") && DontAcessEvaluatedAssessment(context))
            {
                // Nega acesso a usuários de acessarem avaliações ocultas.
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Assessments" }, {"action", "Index" } });
            }
        }

        base.OnActionExecuted(context);
    }

    private static bool DontAcessEvaluatedAssessment(ActionExecutedContext context)
    {
        int id = Convert.ToInt32(context.HttpContext.Request.RouteValues["id"]);

        using SEP_WebContext _database = new();

        Assessment assessment = _database.Assessments.Find(id);

        CivilServant servant = _database.Servants.Where(x => x.Id == assessment.CivilServantId).FirstOrDefault();

        if(assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED && servant.UserStats == UserStatsEnum.UnderLicense)
        {
            return true;
        }

        return false;
    }

    private static bool IsEvaluationAlreadyEvaluated(ActionExecutedContext context)
    {
        int id = Convert.ToInt32(context.HttpContext.Request.RouteValues["id"]);

        using SEP_WebContext _database = new();

        Assessment assessment = _database.Assessments.Find(id);

        CivilServant servant = _database.Servants.Where(x => x.Id == assessment.CivilServantId).FirstOrDefault();

        if(assessment.Stats == AssessmentStatsEnum.EVALUATED)
        {
            return true;
        }
        else if(assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED)
        {
            return false;     
        }
        else
        {
            return false;
        }
    }
}