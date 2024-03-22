using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Models;

namespace SEP_Web.Filters;
public interface IFilterServices
{
    Users GetUserFromSession(ActionExecutedContext context);
    void RedirectToLogin(ActionExecutedContext context);
    void RedirectToAssessments(ActionExecutedContext context);
}