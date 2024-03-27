using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEP_Web.Keys;

namespace SEP_Web.Models;

[Table("Assessments")]
public class Assessment
{
    public const int APT = 1;
    public const int INAPT = 0;
    
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O status da avaliação é obrigatório !")]
    public AssessmentStatsEnum Stats { get; set; }

    [Required(ErrorMessage = "A etapa da avaliação é obrigatória !")]
    public int Phase { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? StartEvaluationPeriod { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? EndEvaluationPeriod { get; set; }

    [ForeignKey("CivilServantId"), Required(ErrorMessage = "Informe o servidor avaliado !")]
    public int CivilServantId { get; set; }
    public CivilServant CivilServant { get; set; }

    [ForeignKey("UserEvaluatorFirst"), Required(ErrorMessage = "Informe o avaliador 1 responsável !")]
    public int? UserEvaluatorId1 { get; set; }
    public UserEvaluator UserEvaluatorFirst { get; set; }

    [ForeignKey("UserEvaluatorSecond"), Required(ErrorMessage = "Informe o avaliador 2 responsável !")]
    public int? UserEvaluatorId2 { get; set; }
    public UserEvaluator UserEvaluatorSecond { get; set; }

    public string EvaluatedFor { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula I em 'Assiduidade' !")]
    public int Crit1_Clau1 { get; set; }
    public string Jus_Crit1_Clau1 { get; set; }

    public int Tot_Crit1 { get; set; }
    public double Average_Crit1 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Disciplina' !")]
    public int Crit2_Clau1 { get; set; }
    public string Jus_Crit2_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Disciplina' !")]
    public int Crit2_Clau2 { get; set; }
    public string Jus_Crit2_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Disciplina' !")]
    public int Crit2_Clau3 { get; set; }
    public string Jus_Crit2_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Disciplina' !")]
    public int Crit2_Clau4 { get; set; }
    public string Jus_Crit2_Clau4 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula V em 'Disciplina' !")]
    public int Crit2_Clau5 { get; set; }
    public string Jus_Crit2_Clau5 { get; set; }

    public int Tot_Crit2 { get; set; }
    public double Average_Crit2 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Iniciativa' !")]
    public int Crit3_Clau1 { get; set; }
    public string Jus_Crit3_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Iniciativa' !")]
    public int Crit3_Clau2 { get; set; }
    public string Jus_Crit3_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Iniciativa' !")]
    public int Crit3_Clau3 { get; set; }
    public string Jus_Crit3_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Iniciativa' !")]
    public int Crit3_Clau4 { get; set; }
    public string Jus_Crit3_Clau4 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula V em 'Iniciativa' !")]
    public int Crit3_Clau5 { get; set; }
    public string Jus_Crit3_Clau5 { get; set; }

    public int Tot_Crit3 { get; set; }
    public double Average_Crit3 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Responsabilidade' !")]
    public int Crit4_Clau1 { get; set; }
    public string Jus_Crit4_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Responsabilidade' !")]
    public int Crit4_Clau2 { get; set; }
    public string Jus_Crit4_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Responsabilidade' !")]
    public int Crit4_Clau3 { get; set; }
    public string Jus_Crit4_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Responsabilidade' !")]
    public int Crit4_Clau4 { get; set; }
    public string Jus_Crit4_Clau4 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula V em 'Responsabilidade' !")]
    public int Crit4_Clau5 { get; set; }
    public string Jus_Crit4_Clau5 { get; set; }

    public int Tot_Crit4 { get; set; }
    public double Average_Crit4 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Produtividade' !")]
    public int Crit5_Clau1 { get; set; }
    public string Jus_Crit5_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Produtividade' !")]
    public int Crit5_Clau2 { get; set; }
    public string Jus_Crit5_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Produtividade' !")]
    public int Crit5_Clau3 { get; set; }
    public string Jus_Crit5_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Produtividade' !")]
    public int Crit5_Clau4 { get; set; }
    public string Jus_Crit5_Clau4 { get; set; }
    
    [Required(ErrorMessage = "Avalie a cláusula V em 'Produtividade' !")]
    public int Crit5_Clau5 { get; set; }
    public string Jus_Crit5_Clau5 { get; set; }

    public int Tot_Crit5 { get; set; }
    public double Average_Crit5 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Relacionamento Interpessoal' !")]
    public int Crit6_Clau1 { get; set; }
    public string Jus_Crit6_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Relacionamento Interpessoal' !")]
    public int Crit6_Clau2 { get; set; }
    public string Jus_Crit6_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Relacionamento Interpessoal' !")]
    public int Crit6_Clau3 { get; set; }
    public string Jus_Crit6_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Relacionamento Interpessoal' !")]
    public int Crit6_Clau4 { get; set; }
    public string Jus_Crit6_Clau4 { get; set; }
    
    [Required(ErrorMessage = "Avalie a cláusula V em 'Relacionamento Interpessoal' !")]
    public int Crit6_Clau5 { get; set; }
    public string Jus_Crit6_Clau5 { get; set; }

    public int Tot_Crit6 { get; set; }
    public double Average_Crit6 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Zelo com o Patrimônio' !")]
    public int Crit7_Clau1 { get; set; }
    public string Jus_Crit7_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Zelo com o Patrimônio' !")]
    public int Crit7_Clau2 { get; set; }
    public string Jus_Crit7_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Zelo com o Patrimônio' !")]
    public int Crit7_Clau3 { get; set; }
    public string Jus_Crit7_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Zelo com o Patrimônio' !")]
    public int Crit7_Clau4 { get; set; }
    public string Jus_Crit7_Clau4 { get; set; }
    
    [Required(ErrorMessage = "Avalie a cláusula V em 'Zelo com o Patrimônio' !")]
    public int Crit7_Clau5 { get; set; }
    public string Jus_Crit7_Clau5 { get; set; }

    public int Tot_Crit7 { get; set; }
    public double Average_Crit7 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Pontualidade' !")]
    public int Crit8_Clau1 { get; set; }
    public string Jus_Crit8_Clau1 { get; set; }

    public int Tot_Crit8 { get; set; }
    public double Average_Crit8 { get; set; }


    [Required(ErrorMessage = "Avalie a cláusula I em 'Eficiência' !")]
    public int Crit9_Clau1 { get; set; }
    public string Jus_Crit9_Clau1 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula II em 'Eficiência' !")]
    public int Crit9_Clau2 { get; set; }
    public string Jus_Crit9_Clau2 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula III em 'Eficiência' !")]
    public int Crit9_Clau3 { get; set; }
    public string Jus_Crit9_Clau3 { get; set; }

    [Required(ErrorMessage = "Avalie a cláusula IV em 'Eficiência' !")]
    public int Crit9_Clau4 { get; set; }
    public string Jus_Crit9_Clau4 { get; set; }
    
    [Required(ErrorMessage = "Avalie a cláusula V em 'Eficiência' !")]
    public int Crit9_Clau5 { get; set; }
    public string Jus_Crit9_Clau5 { get; set; }

    public int Tot_Crit9 { get; set; }
    public double Average_Crit9 { get; set; }


    public int MedicalRestriction { get; set; }

    [StringLength(40)]
    public string Crit10_Justification{ get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ForwardingDate { get; set; }

    [Required(ErrorMessage = "informe uma data de registro !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime RegisterDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? ModifyDate { get; set; }

    [StringLength(35)]
    public string LastModifiedBy { get; set; }

    public double Grand_Tot { get; set; }
    public double Overall_Average { get; set; }

    public int? AssessmentResult { get; set; }
}
