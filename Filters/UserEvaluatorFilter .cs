using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Keys;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Filters;
public class UserEvaluatorFilter : ActionFilterAttribute
{
    private readonly IFilterServices _filters;

    public UserEvaluatorFilter(IFilterServices filters)
    {
        _filters = filters;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        Users user = _filters.GetUserFromSession(context);

        if (user == null || user.UserStats != UserStatsEnum.Active)
        {
            _filters.RedirectToLogin(context);
            return;
        }

        int? userType = context.HttpContext.Session.GetInt32("userType");

        if (userType == null || (UsersTypeEnum)userType != UsersTypeEnum.User_Evaluator && (UsersTypeEnum)userType != UsersTypeEnum.User_Admin)
        {
            _filters.RedirectToLogin(context);
            return;
        }

        base.OnActionExecuted(context);
    }
}