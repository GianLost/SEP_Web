using SEP_Web.Models.UsersModels;
using SEP_Web.Models.PasswordModels;

namespace SEP_Web.Services;

/// <summary>
///     Interface responsável por operações em comum relacionadas aos usuários no sistema.
/// </summary>

public interface IUsersServices
{
    /// <summary>
    ///     Busca um usuário pelo ID e MASP para realizar alteração de senha.
    /// </summary>
    /// <param name="id">ID do usuário a ser buscado.</param>
    /// <param name="masp">MASP do usuário a ser buscado.</param>
    /// <returns>O usuário correspondente ao ID e MASP fornecidos.</returns>
    Users SearchUserToChangePass(int id, int masp); // Busca um usuário pelo Id e MASP;

    /// <summary>
    ///     Altera a senha de um usuário.
    /// </summary>
    /// <param name="changePassword">Dados necessários para alteração da senha.</param>
    /// <returns>Uma tarefa assíncrona representando a conclusão da alteração de senha.</returns>
    Task<Users> ChangePassword(ChangePassword changePassword); // Alterar senha de um usuário;
}