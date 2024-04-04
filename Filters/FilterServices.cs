using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SEP_Web.Models;

namespace SEP_Web.Filters;

public class FilterServices : IFilterServices
{
    private const string UserSessionKey = "userCheckIn";
    private const string ErrorMessageKey = "ErrorMessage";
    private readonly ILogger<IFilterServices> _logger;
    private readonly ITempDataDictionaryFactory _tempData;

        public FilterServices(ILogger<FilterServices> logger, ITempDataDictionaryFactory tempData)
        {
            _logger = logger;
            _tempData = tempData;
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

    public void RedirectToAssessments(ActionExecutedContext context, string errorMessage)
    {
        SetErrorMessage(context, errorMessage);
        context.Result = new RedirectToActionResult("Index", "Assessments", null);
    }

    public void RedirectToLogin(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Logout", "Login", null);
    }

    public void RedirectToHome(ActionExecutedContext context)
    {
        context.Result = new RedirectToActionResult("Index", "Home", null);
    }

    private void SetErrorMessage(ActionExecutedContext context, string message)
    {
        var tempData = _tempData.GetTempData(context.HttpContext);
        tempData[ErrorMessageKey] = message;
    }
}
