using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models;

[Table("Divisions")]
public class Division : Structures
{
    [ForeignKey("InstituitionId"), Required(ErrorMessage = "Selecione um órgão !")]
    public int InstituitionId { get; set; }
    public Instituition Instituition { get; set; }
}