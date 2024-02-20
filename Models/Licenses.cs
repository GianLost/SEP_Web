using System.ComponentModel.DataAnnotations;

namespace SEP_Web.Models;
public class Licenses
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da licença é obrigatório !")]
    [StringLength(50), MaxLength(50, ErrorMessage = "O nome da licença não pode conter mais de 50 caractéres.")]
    [MinLength(3, ErrorMessage = "O nome da licença não pode conter menos de 3 caractéres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O tempo da licença é obrifatório !")]
    public int? Time { get; set; }

}
