using Microsoft.AspNetCore.Mvc;
using SEP_Web.Models;

namespace SEP_Web.Helper.Validation;

/// <summary>
/// Interface para operações de validação de usuários.
/// </summary>
public interface IUsersValidation
{
    /// <summary>
    /// Verifica se um campo com um determinado valor existe em qualquer uma das tabelas de usuário.
    /// </summary>
    /// <param name="fieldName">O nome do campo a ser verificado.</param>
    /// <param name="value">O valor a ser verificado.</param>
    /// <returns>Verdadeiro se o campo existe com o valor fornecido, caso contrário, falso.</returns>
    Task<bool> CheckIfFieldExistsInAnyUserTable(string fieldName, object value);

    /// <summary>
    /// Verifica campos de usuário duplicados no banco de dados.
    /// </summary>
    /// <param name="user">Objeto de usuário para verificar duplicatas.</param>
    /// <returns>Uma lista de nomes de campos duplicados e mensagens de erro correspondentes.</returns>
    Task<List<(string FieldName, string Message)>> CheckForDuplicateUserFields(Users user);

    /// <summary>
    /// Armazena mensagens de erro de validação dos campos de login em um TempData.
    /// </summary>
    /// <param name="fields">A chave que corresponde ao campo validade para armazenar em "TempData".</param>
    /// <param name="errorMessage">A mensagem de erro a ser armazenada para ser retornada ao usuário.</param>
    /// <param name="controller">Controlador associado à solicitação.</param>
    void LoginFieldsValidation(string fields, string errorMessage, Controller controller);

    /// <summary>
    /// Valida se a senha fornecida corresponde à senha confirmada.
    /// </summary>
    /// <param name="pass">Senha a ser validada.</param>
    /// <param name="confirmPass">Confirmação da senha para comparação.</param>
    /// <param name="controller">Controlador associado à solicitação.</param>
    /// <returns>Verdadeiro se as senhas coincidirem, caso contrário, falso.</returns>
    bool ValidatePassword(string pass, string confirmPass, Controller controller);

    /// <summary>
    /// Utilizado para as edições de usuários, verifica se o valor de um campo específico foi alterado.
    /// </summary>
    /// <param name="existingUser">Objeto de usuário que irá passar por edição de dados.</param>
    /// <param name="fieldName">Nome do campo a ser verificado.</param>
    /// <param name="newValue">Novo valor atribuído que será comparado.</param>
    /// <returns>Verdadeiro se o campo foi alterado, caso contrário, falso.</returns>
    bool IsFieldChanged(Users existingUser, string fieldName, object newValue);
}
