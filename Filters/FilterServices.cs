using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Models;

namespace SEP_Web.Filters;

public class FilterServices : IFilterServices
{
    private const string UserSessionKey = "userCheckIn";
    private readonly ILogger<IFilterServices> _logger;

    public FilterServices(ILogger<IFilterServices> logger)
    {
        _logger = logger;
    }

    public Users GetUserFromSession(ActionExecutedContext context)
    {
        string userSession = context.HttpContext.Session.GetString(UserSessionKey);

        if (string.IsNullOrEmpty(userSession))
        {
            RedirectToLogin(context);
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<Users>(userSession);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Erro ao desserializar o usuário na sessão.");
            RedirectToLogin(context);
            return null;
        }
    }

    public void RedirectToAssessments(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Assessments", null);
    }

    public void RedirectToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Login", null);
    }
}
