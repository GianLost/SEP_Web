using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Models.StructuresModels;
public class Structures
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe uma descrição !")]
    [StringLength(70, MinimumLength = 3, ErrorMessage = "O campo Nome do órgão deve ter entre 3 e 70 caracteres.")]
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

    [ForeignKey("UserAdministratorId")]
    public int UserAdministratorId { get; set; }
    public UserAdministrator UserAdministrator { get; set; }
}