namespace SEP_Web.Keys;

/// <summary>
///     <para>Enumera os possíveis estados de um usuário no sistema:</para>
///     <para><b>- Active:</b> Representa um usuário registrado e habilitado para acessar o sistema.</para>
///     <para><b>- Inactive:</b> Representa um usuário cujo acesso foi desabilitado, negando permissões de acesso ao sistema.</para>
/// </summary>

public enum UserStatsEnum
{
    UnderLicense = 2,
    Active = 1,
    Inactive = 0,
}