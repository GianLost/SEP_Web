using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEP_Web.Models.StructuresModels;

namespace SEP_Web.Models.UsersModels;

/// <summary>
///     <para><b>Avaliadores:</b></para>
///     <para>- Os avaliadores herdam seus atributos da classe base <b>SEP_Web.Models.Users</b> e também possuem atributos próprios.
///     Eles possuem permissão de acesso e modificação apenas sobre si mesmos e sobre os servidores públicos.</para>
///     <para>- A classe <b>UserEvalator</b> é usada para mapear uma tabela do banco de dados,
///     representando uma entidade gerada pelo <b>EF Core</b>.</para>
///     <para><b>Nome da Tabela:</b></para>
///     <para>- Evaluators;</para>
/// </summary>

[Table("Evaluators")]
public class UserEvaluator : Users
{
    [ForeignKey("InstituitionId"), Required(ErrorMessage = "Selecione um órgão !")]
    public int? InstituitionId { get; set; }
    public Instituition Instituition { get; set; }

    [ForeignKey("DivisionId"), Required(ErrorMessage = "Selecione uma divisão !")]
    public int? DivisionId { get; set; }
    public Division Division { get; set; }

    [ForeignKey("SectionId"), Required(ErrorMessage = "Selecione uma seção !")]
    public int? SectionId { get; set; }
    public Section Section { get; set; }

    [ForeignKey("SectorId"), Required(ErrorMessage = "Selecione um setor !")]
    public int? SectorId { get; set; }
    public Sector Sector { get; set; }
}