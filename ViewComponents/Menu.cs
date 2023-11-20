using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SEP_Web.Models;

namespace SEP_Web.ViewComponents;

// Classe responsável por exibir o componente de Menu em /Shared/Components que contem o menu de navegação da aplicação;
public class Menu : ViewComponent //  A classe menu herda de ViewComponent;
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string userSession = HttpContext.Session.GetString("userCheckIn"); // Captura a variável serializada de usuário armazenada na sessão;

        if (string.IsNullOrEmpty(userSession)) return null; // verifica se é nula ou vazia;

        using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(userSession)); // utiliza uma instância de um MemoryStream passando como parâmetro os bytes da string de sessão;

        Users users = await JsonSerializer.DeserializeAsync<Users>(memoryStream); // os dados correspondentes são desserializados em um objto users utilizando o JsonSerializer;

        return View(users); // Retorna o objeto dessesrializado;
    }

}