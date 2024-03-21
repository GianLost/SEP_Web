using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Filters;
public class LoggedinUserFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString("userCheckIn");

        if (string.IsNullOrEmpty(userSession))
        {
            ReturnToLogin(context);
            return;
        }
        else
        {
            Users user = JsonSerializer.Deserialize<Users>(userSession);

            if (user == null)
            {
                ReturnToLogin(context);
                return;
            }

            if (user.UserStats != UserStatsEnum.Active)
            {
                ReturnToLogin(context);
                return;
            }
        }

        base.OnActionExecuted(context);
    }

    private static void ReturnToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Login", null);
    }
}
