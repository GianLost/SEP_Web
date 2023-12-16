using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Keys;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Controllers;

public class ChangePasswordController : Controller
{
    private readonly ILogger<ChangePasswordController> _logger;
    private readonly IUsersServices _usersServices;
    private readonly IUserSession _session;

    public ChangePasswordController(ILogger<ChangePasswordController> logger, IUsersServices usersServices, IUserSession session)
    {
        _logger = logger;
        _usersServices = usersServices;
        _session = session;
    }

    [HttpPost]
    public async Task<IActionResult> ChangeUserPassword(ChangePassword changePassword)
    {
        try
        {
            Users userInSession = await _session.SearchUserSession();

            if (!ModelState.IsValid)
                return Json(new { stats = StatsAJAXEnum.ERROR});

                if(changePassword.Password != changePassword.ComparePassword)
                    return Json(new { stats = StatsAJAXEnum.INVALID });

                    if(changePassword.Password.Length < 8 || changePassword.ComparePassword.Length < 8)
                        return Json(new { stats = StatsAJAXEnum.INVALID_LENGTH });

            changePassword.LastModifiedBy = userInSession.Login;
            await _usersServices.ChangePassword(changePassword);

            if(changePassword.Masp == userInSession.Masp)
            {
                _session.UserCheckOut();
                return Json(new { stats = StatsAJAXEnum.END_SESSION });
            }

            TempData["SuccessMessage"] = "Senha editada com sucesso.";
            return Json(new { stats = StatsAJAXEnum.OK });

        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Não foi possível editar a senha.";
            _logger.LogError("Não foi possível editar a senha", ex.Message);
            return Json(new { stats = StatsAJAXEnum.ERROR, message = "Ocorreu um erro ao editar a senha!" });
        }
    }
}