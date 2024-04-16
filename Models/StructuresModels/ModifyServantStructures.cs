using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Models.StructuresModels;

public class ModifyServantStructures : ModifyStructures
{
    [ForeignKey("UserEvaluatorFirst"), Required(ErrorMessage = "Informe o Avaliador 1 !")]
    public int? UserEvaluatorId1 { get; set; }
    public UserEvaluator UserEvaluatorFirst { get; set; }

    [ForeignKey("UserEvaluatorSecond"), Required(ErrorMessage = "Informe o Avaliador 2 !")]
    public int? UserEvaluatorId2 { get; set; }
    public UserEvaluator UserEvaluatorSecond { get; set; }
}