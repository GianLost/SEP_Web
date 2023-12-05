using Microsoft.AspNetCore.Mvc;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Helper.Validation;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserSession _session;
    private readonly IUsersValidation _validation;

    public LoginController(ILogger<LoginController> logger, IUserSession session, IUsersValidation validation)
    {
        _logger = logger;
        _session = session;
        _validation = validation;
    }

    public async Task<IActionResult> Index()
    {
        if (await _session.SearchUserSession() != null) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(Login login)
    {
        try
        {
            if (ModelState.IsValid)
            {
                Users users = await _session.UserSignIn(login.Masp, login.LoginName, this);

                if (!(users == null))
                {
                    if (login.Masp != users.Masp)
                    {
                        _validation.LoginFieldsValidation("InvalidMASP", "O MASP informado é inválido", this);
                        return View("Index");
                    }

                    if (login.LoginName != users.Login)
                    {
                        _validation.LoginFieldsValidation("InvalidLogin", "O login informado é inválido", this);
                        return View("Index");
                    }

                    if (!Cryptography.VerifyPasswordEncrypted(login.Password, users.Password))
                    {
                        _validation.LoginFieldsValidation("InvalidPass", "A senha informada é inválida", this);
                        return View("Index");
                    }

                    if (users.UserStats != UserStatsEnum.Active)
                    {
                        TempData["ErrorMessage"] = FeedbackMessages.ErrorAccountDisable;
                        return View("Index");
                    }
                    else
                    {
                        _session.UserCheckIn(users);
                        return RedirectToAction("Index", "Home");
                    }

                }

                TempData["ErrorMessage"] = ExceptionMessages.LogInDataNull;

            }

            return View("Index");

        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"{FeedbackMessages.ErrorLogin} {ExceptionMessages.ErrorDatabaseConnection}";
            _logger.LogError(FeedbackMessages.ErrorLogin, e.Message);
            return RedirectToAction("Index");
        }
    }
    public IActionResult Logout()
    {
        _session.UserCheckOut(); // UserCheckOut finaliza e remove os dados da sessão;
        return RedirectToAction("Index", "Login");
    }

}
