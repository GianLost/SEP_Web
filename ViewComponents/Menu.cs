using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SEP_Web.Models;

namespace SEP_Web.ViewComponents;

// Classe responsável por exibir o componente de Menu em /Shared/Components que contem o menu de navegação da aplicação;
public class Menu : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Menu(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string userSession = _httpContextAccessor.HttpContext.Session.GetString("userCheckIn");

        if (string.IsNullOrEmpty(userSession))
        {
            // Obter o URL para a action SignIn da Controller de Login
            var signInUrl = Url.Action("Index", "Login");

            // Retornar um ViewComponentResult indicando a necessidade de redirecionamento
            return View("Redirect", signInUrl);
        }

        using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(userSession));
        Users users = await JsonSerializer.DeserializeAsync<Users>(memoryStream);

        return View(users);
    }
}