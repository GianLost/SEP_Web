using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Helper.Authentication;
public class UserSession : IUserSession
{
    private readonly SEP_WebContext _database;
    private readonly IHttpContextAccessor _httpContext;

    public UserSession(IHttpContextAccessor httpContext, SEP_WebContext database)
    {
        _httpContext = httpContext;
        _database = database;
    }

    public async Task<Users> SearchUserSession()
    {
        string userSession = _httpContext.HttpContext.Session.GetString("userCheckIn");

        if (string.IsNullOrEmpty(userSession)) return null;

        Users user = new();

        if (userSession.Contains("UserType") == Convert.ToBoolean(UsersTypeEnum.User_Admin))
        {
            user = await JsonSerializer.DeserializeAsync<UserAdministrator>(new MemoryStream(Encoding.UTF8.GetBytes(userSession)));
        }
        else if (userSession.Contains("UserType") == Convert.ToBoolean(UsersTypeEnum.User_Evaluator))
        {
            user = await JsonSerializer.DeserializeAsync<UserEvaluator>(new MemoryStream(Encoding.UTF8.GetBytes(userSession)));
        }

        return user;
    }

    public void UserCheckIn(Users users)
    {
        string value = JsonSerializer.Serialize(users);

        _httpContext.HttpContext.Session.SetString("userCheckIn", value);
        _httpContext.HttpContext.Session.SetInt32("userType", (int)users.UserType);
        _httpContext.HttpContext.Session.SetInt32("userId", (int)users.Id);
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
        _httpContext.HttpContext.Session.Remove("userCheckIn");
        _httpContext.HttpContext.Session.Clear();
    }
}
