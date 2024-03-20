using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;
public class UserEvaluatorFilter : ActionFilterAttribute
{
    private const string UserSessionKey = "userCheckIn";

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString(UserSessionKey);
        int? userType = context.HttpContext.Session.GetInt32("userType");
        int? userStats = context.HttpContext.Session.GetInt32("userStats");

        if (string.IsNullOrEmpty(userSession))
        {
            RedirectToLogin(context);
            return;
        }

        if (userType == null || userStats == null)
        {
            RedirectToLogin(context);
            return;
        }

        if (userType == (int)UsersTypeEnum.User_Admin)
        {
            UserAdministrator administrator = JsonSerializer.Deserialize<UserAdministrator>(userSession);

            if (administrator == null || administrator.UserStats != UserStatsEnum.Active)
            {
                RedirectToLogin(context);
                return;
            }
        }
        else if (userType == (int)UsersTypeEnum.User_Evaluator)
        {
            Users users = JsonSerializer.Deserialize<Users>(userSession);

            if (users != null)
            {
                using SEP_WebContext _database = new();
                UserEvaluator evaluator = _database.Evaluators.FirstOrDefault(x => x.Id == users.Id && x.Name == users.Name);

                if (evaluator == null || evaluator.UserStats != UserStatsEnum.Active)
                {
                    RedirectToLogin(context);
                    return;
                }
            }
        }

        base.OnActionExecuted(context);
    }

    private static void RedirectToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Login", null);
    }
}