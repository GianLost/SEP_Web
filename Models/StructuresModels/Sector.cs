using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models.StructuresModels;

[Table("Sectors")]
public class Sector : Structures
{
    [ForeignKey("SectionId"), Required(ErrorMessage = "Selecione uma seção !")]
    public int SectionId { get; set; }
    public Section Section { get; set; }
}