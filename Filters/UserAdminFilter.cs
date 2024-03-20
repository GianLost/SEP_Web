using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;

public class UserAdminFilter : ActionFilterAttribute
{
    private const string UserSessionKey = "userCheckIn";

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString(UserSessionKey);
        
        if (string.IsNullOrEmpty(userSession))
        {
            RedirectToLogin(context);
            return;
        }

        Users user = JsonSerializer.Deserialize<Users>(userSession);

        if (user == null || user.UserStats != UserStatsEnum.Active || user.UserType != UsersTypeEnum.User_Admin)
        {
            RedirectToLogin(context);
            return;
        }

        base.OnActionExecuted(context);
    }

    private static void RedirectToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Login", null);
    }

}