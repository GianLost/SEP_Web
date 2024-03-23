using Microsoft.AspNetCore.Mvc.Filters;
using SEP_Web.Models;

namespace SEP_Web.Filters;

/// <summary>
/// Interface para serviços de filtros de permissão.
/// </summary>
public interface IFilterServices
{
    /// <summary>
    /// Obtém informações do usuário da sessão.
    /// </summary>
    /// <param name="context">O contexto da execução da ação.</param>
    /// <returns>O usuário recuperado da sessão ou null se não for encontrado.</returns>
    Users GetUserFromSession(ActionExecutedContext context);

    /// <summary>
    /// Redireciona o usuário para a página de login.
    /// </summary>
    /// <param name="context">Realiza o redirecionamento de acordo com o contexto de execução da ação.</param>
    void RedirectToLogin(ActionExecutedContext context);

    /// <summary>
    /// Redireciona o usuário para a página inicial.
    /// </summary>
    /// <param name="context">Realiza o redirecionamento de acordo com o contexto de execução da ação.
    void RedirectToHome(ActionExecutedContext context);

    /// <summary>
    /// Redireciona o usuário para a página de listagem de avaliações.
    /// </summary>
    /// <param name="context">Realiza o redirecionamento de acordo com o contexto de execução da ação.
    void RedirectToAssessments(ActionExecutedContext context);
}