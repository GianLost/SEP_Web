using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEP_Web.Models;

[Table("ServantLicense")]
public class ServantLicense
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [ForeignKey("Servant"), Required(ErrorMessage = "Informe o servidor !")]
    public int? CivilServantId { get; set; }
    public CivilServant CivilServant { get; set; }

    [ForeignKey("LicensesId")]
    public int? LicensesId { get; set; }
    public Licenses License { get; set; } 

    [Required(ErrorMessage = "Informe a data de início !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Informe a data de fim !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "informe uma data de registro !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime RegisterDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ModifyDate { get; set; }

    [StringLength(35)]
    public string LastModifiedBy { get; set; }
}