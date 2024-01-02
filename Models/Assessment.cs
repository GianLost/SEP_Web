using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEP_Web.Keys;

namespace SEP_Web.Models;

[Table("Assessments")]
public class Assessment
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O status da avaliação é obrigatório !")]
    public AssessmentStatsEnum Stats { get; set; }

    [Required(ErrorMessage = "A etapa da avaliação é obrigatória !")]
    public int Phase { get; set; }

    [Required(ErrorMessage = "O campo masp é obrigatório !")]
    public int? Masp { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? StartEvaluationPeriod { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? EndEvaluationPeriod { get; set; }

    [ForeignKey("CivilServantId"), Required(ErrorMessage = "Informe o servidor avaliado !")]
    public int CivilServantId { get; set; }
    public CivilServant CivilServant { get; set; }

    [ForeignKey("UserEvaluatorId"), Required(ErrorMessage = "Informe o avaliador responsável !")]
    public int? UserEvaluatorId { get; set; }
    public UserEvaluator UserEvaluator { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Desempenho' !")]
    public int Crit1_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Desempenho' !")]
    public int Crit1_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Desempenho' !")]
    public int Crit1_Clau3 { get; set; }

    public int Tot_Crit1 { get; set; }
    public double Average_Crit1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula I em 'Adaptação' !")]
    public int Crit2_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Adaptação' !")]
    public int Crit2_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Adaptação' !")]
    public int Crit2_Clau3 { get; set; }

    public int Tot_Crit2 { get; set; }
    public double Average_Crit2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula I em 'Proatividade' !")]
    public int Crit3_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Proatividade' !")]
    public int Crit3_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Produtividade' !")]
    public int Crit3_Clau3 { get; set; }

    public int Tot_Crit3 { get; set; }
    public double Average_Crit3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula I em 'Cumprimento de Prazos' !")]
    public int Crit4_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Cumprimento de Prazos' !")]
    public int Crit4_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Cumprimento de Prazos' !")]
    public int Crit4_Clau3 { get; set; }

    public int Tot_Crit4 { get; set; }
    public double Average_Crit4 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Habilidades Técnicas' !")]
    public int Crit5_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Habilidades Técnicas' !")]
    public int Crit5_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Habilidades Técnicas' !")]
    public int Crit5_Clau3 { get; set; }

    public int Tot_Crit5 { get; set; }
    public double Average_Crit5 { get; set; }

    public int Grand_Tot { get; set; }
    public double Overall_Average { get; set; }
}
