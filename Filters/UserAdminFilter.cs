using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;

public class UserAdminFilter : ActionFilterAttribute
{
    private const string UserSessionKey = "userCheckIn";

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString(UserSessionKey);
        string url = context.HttpContext.Request.Path.Value;

        if (string.IsNullOrEmpty(userSession))
        {
            RedirectToLogin(context);
            return;
        }

        Users user = JsonSerializer.Deserialize<Users>(userSession);

        if (user == null || user.UserStats != UserStatsEnum.Active)
        {
            RedirectToLogin(context);
            return;
        }

        if (url.Contains("ToAssess"))
        {
            var assessmentId = GetAssessmentId(context);

            if (assessmentId.HasValue)
            {
                using var _database = new SEP_WebContext();
                var assessment = _database.Assessments.Find(assessmentId.Value);

                if (assessment != null)
                {
                    var servant = _database.Servants.FirstOrDefault(x => x.Id == assessment.CivilServantId);

                    if (DontAccessEvaluatedAssessment(assessment))
                    {
                        RedirectToAssessmentsIndex(context);
                        return;
                    }else if(DontAccessAssessmentIsNotEvaluator(user, servant, assessment))
                    {
                        RedirectToAssessmentsIndex(context);
                        return;
                    }else if(servant.UserStats == UserStatsEnum.UnderLicense)
                    {
                        RedirectToAssessmentsIndex(context);
                        return;
                    }else
                    {
                        return;
                    }
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

    private static bool DontAccessEvaluatedAssessment(Assessment assessment)
    {
        if (assessment.Stats == AssessmentStatsEnum.EVALUATED)
        {
            return true;
        }
        else if (assessment.Stats == AssessmentStatsEnum.NOT_EVALUATED)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    private static bool DontAccessAssessmentIsNotEvaluator(Users user, CivilServant servant, Assessment assessment)
    {
        return user.UserType != UsersTypeEnum.User_Admin && servant.Id != assessment.UserEvaluatorId1 || servant.Id != assessment.UserEvaluatorId2;
    }

    private static void RedirectToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
    }

    private static void RedirectToAssessmentsIndex(ActionExecutedContext context)
    {
        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Assessments" }, { "action", "Index" } });
    }
}