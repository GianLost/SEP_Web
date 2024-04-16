using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models.StructuresModels;

public class ModifyStructures
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório!")]
    public int Id { get; set; }

    [ForeignKey("InstituitionId"), Required(ErrorMessage = "Selecione um órgão!")]
    public int? InstituitionId { get; set; }
    public Instituition Instituition { get; set; }

    [ForeignKey("DivisionId"), Required(ErrorMessage = "Selecione uma divisão!")]
    public int? DivisionId { get; set; }
    public Division Division { get; set; }

    [ForeignKey("SectionId"), Required(ErrorMessage = "Selecione uma seção!")]
    public int? SectionId { get; set; }
    public Section Section { get; set; }

    [ForeignKey("SectorId"), Required(ErrorMessage = "Selecione um setor!")]
    public int? SectorId { get; set; }
    public Sector Sector { get; set; }
}