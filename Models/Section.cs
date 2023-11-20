using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models;

[Table("Sections")]
public class Section : Structures
{
    [ForeignKey("DivisionId"), Required(ErrorMessage = "Selecione uma divisão !")]
    public int DivisionId { get; set; }
    public Division Division { get; set; }
}