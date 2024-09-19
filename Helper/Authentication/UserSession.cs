using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Helper.Authentication;
public class UserSession : IUserSession
{
    private readonly ILogger<IUserSession> _logger;
    private readonly SEP_WebContext _database;
    private readonly IHttpContextAccessor _httpContext;
    private const string UserSessionKey = "userCheckIn";

    public UserSession(IHttpContextAccessor httpContext, SEP_WebContext database, ILogger<IUserSession> logger)
    {
        _logger = logger;
        _httpContext = httpContext;
        _database = database;
    }

    public async Task<Users> SearchUserSession()
    {
        string userSession = _httpContext.HttpContext.Session.GetString(UserSessionKey);

        if (string.IsNullOrEmpty(userSession)) return null;

        try
        {
            return await JsonSerializer.DeserializeAsync<Users>(new MemoryStream(Encoding.UTF8.GetBytes(userSession)));
        }
        catch(JsonException ex)
        {
            _logger.LogError(ex, "Erro ao tentar desserializar o objeto de usuário para estabelecer a sessão.");
            return null;
        }
    }

    public void UserCheckIn(Users users)
    {
        string value = JsonSerializer.Serialize(users);

        _httpContext.HttpContext.Session.SetString("userCheckIn", value);
        _httpContext.HttpContext.Session.SetInt32("userType", (int)users.UserType);
        _httpContext.HttpContext.Session.SetInt32("userId", users.Id);
        _httpContext.HttpContext.Session.SetInt32("userStats", (int)users.UserStats);
    }

    public async Task<Users> UserSignIn(int? masp, string login, Controller controller)
    {
        Users user = await _database.Administrators.FirstOrDefaultAsync(x => x.Login.ToUpper() == login.ToUpper() || x.Masp == masp);
        user ??= await _database.Evaluators.FirstOrDefaultAsync(x => x.Login.ToUpper() == login.ToUpper() || x.Masp == masp);

        return user;
    }

    public void UserCheckOut()
    {
        _httpContext.HttpContext.Session.Remove(UserSessionKey);
        _httpContext.HttpContext.Session.Clear();
    }
}
