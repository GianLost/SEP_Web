using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models;

[Table("Servants")]
public class CivilServant : Users
{
    [Required(ErrorMessage = "Informe a data de admissão !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? AdmissionDate { get; set; }

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

    [ForeignKey("UserEvaluatorFirst"), Required(ErrorMessage = "Informe o Avaliador 1 !")]
    public int? UserEvaluatorId1 { get; set; }
    public UserEvaluator UserEvaluatorFirst { get; set; }

    [ForeignKey("UserEvaluatorSecond"), Required(ErrorMessage = "Informe o Avaliador 2 !")]
    public int? UserEvaluatorId2 { get; set; }
    public UserEvaluator UserEvaluatorSecond { get; set; }
}