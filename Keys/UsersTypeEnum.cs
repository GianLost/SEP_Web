namespace SEP_Web.Keys;

/// <summary>
///     <para>Enumera os tipos de usuários disponíveis no sistema:</para>
///     <para><b>- User_Admin:</b> Representa um administrador com acesso total ao sistema.</para>
///     <para><b>- User_Evaluator:</b> Representa um avaliador com permissões específicas para avaliação.</para>
///     <para><b>- User_Server:</b> Representa um usuário com privilégios limitados.</para>
/// </summary>

public enum UsersTypeEnum
{
    User_Admin = 1,
    User_Evaluator = 2,
    User_Server = 3,
}