using SEP_Web.Models.UsersModels;
using Microsoft.AspNetCore.Mvc;

namespace SEP_Web.Helper.Authentication;

/// <summary>
/// Interface para gerenciar a sessão dos usuários.
/// </summary>
public interface IUserSession
{
    /// <summary>
    /// Busca por uma sessão de usuário ativa verificando as variáveis de sessão.
    /// </summary>
    /// <returns>Uma tarefa assíncrona retornando o usuário associado à sessão ativa, se houver.</returns>
    Task<Users> SearchUserSession();

    /// <summary>
    /// Realiza o login de um usuário validando as credenciais fornecidas e estabelecendo uma sessão.
    /// </summary>
    /// <param name="masp">Identificador único do usuário.</param>
    /// <param name="login">Nome de login do usuário.</param>
    /// <param name="controller">Controlador que inicia o processo de login.</param>
    /// <returns>Uma tarefa assíncrona representando o usuário logado, se a autenticação for bem-sucedida.</returns>
    Task<Users> UserSignIn(int? masp, string login, Controller controller);

    /// <summary>
    /// Configura uma sessão de usuário serializando e armazenando informações relevantes do usuário.
    /// </summary>
    /// <param name="users">Objeto de usuário a ser serializado e armazenado na sessão.</param>
    void UserCheckIn(Users users);

    /// <summary>
    /// Encerra a sessão de usuário atual removendo os dados serializados e limpando as variáveis de sessão.
    /// </summary>
    void UserCheckOut();
}