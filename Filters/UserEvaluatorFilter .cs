using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;
public class UserEvaluatorFilter  : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {

        string userSession = context.HttpContext.Session.GetString("userCheckIn");
        int? type = context.HttpContext.Session.GetInt32("userType");
        int? stats = context.HttpContext.Session.GetInt32("userStats");

        if (string.IsNullOrEmpty(userSession))
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
        }
        else
        {
            if (type == Convert.ToInt32(UsersTypeEnum.User_Admin))
            {
                UserAdministrator administrator = JsonSerializer.Deserialize<UserAdministrator>(userSession);

                if(administrator == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                }

                if(administrator.UserStats != UserStatsEnum.Active)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                }
            }
            if (type == Convert.ToInt32(UsersTypeEnum.User_Evaluator))
            {
                Users users = JsonSerializer.Deserialize<Users>(userSession);
                
                if(!(users == null))
                {
                    using SEP_WebContext _database = new();

                    UserEvaluator evaluator = _database.Evaluators.FirstOrDefault(x => x.Id == users.Id && x.Name == users.Name);

                    if(evaluator == null)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                    }

                    if(evaluator.UserStats != UserStatsEnum.Active)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Index" } });
                    }
                }
            }
        }

        base.OnActionExecuted(context);
    }
}