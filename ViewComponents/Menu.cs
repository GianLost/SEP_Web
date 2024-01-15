using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.ViewComponents;

// Classe responsável por exibir o componente de Menu em /Shared/Components que contem o menu de navegação da aplicação;
public class Menu : ViewComponent
{
    private readonly ILogger<Menu> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Menu(IHttpContextAccessor httpContextAccessor, ILogger<Menu> logger)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string userSession = _httpContextAccessor.HttpContext.Session.GetString("userCheckIn");

        if (string.IsNullOrEmpty(userSession))
        {
            string signInUrl = Url.Action("Logout", "Login");
            _logger.LogError(ExceptionMessages.ExpiredSession);
            return View("Redirect", signInUrl);
        }

        using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(userSession));
        Users users = await JsonSerializer.DeserializeAsync<Users>(memoryStream);

        return View(users);
    }
}