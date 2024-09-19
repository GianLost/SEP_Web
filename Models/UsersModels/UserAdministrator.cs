using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models.UsersModels;

/// <summary>
///     <para><b>Administradores:</b></para>
///     <para>- Os administradores herdam seus atributos da classe base <b>SEP_Web.Models.Users</b>.
///     Eles possuem todas as permissões de acesso e modificação sobre os subníveis de usuários
///     e demais estruturas acessíveis do sistema.</para>
///     <para>- A classe <b>UserAdministrator</b> é usada para mapear uma tabela do banco de dados,
///     representando uma entidade gerada pelo <b>EF Core</b>.</para>
///     <para><b>Nome da Tabela:</b></para>
///     <para>- Administrators;</para>
/// </summary>

[Table("Administrators")]
public class UserAdministrator : Users
{

}
