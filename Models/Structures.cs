using System.ComponentModel.DataAnnotations;

namespace SEP_Web.Models;
public class Structures
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe uma descrição !")]
    [StringLength(70), MaxLength(70, ErrorMessage = "A descrição não pode conter mais de 70 caractéres.")]
    [MinLength(3, ErrorMessage = "A descrição não pode conter menos de 3 caractéres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Informe uma data de registro!")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime RegisterDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? ModifyDate { get; set; }

    [StringLength(35)]
    public string LastModifiedBy { get; set; }
}