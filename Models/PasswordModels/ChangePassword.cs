using System.ComponentModel.DataAnnotations;

namespace SEP_Web.Models.PasswordModels;

public class ChangePassword
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório!")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo MASP é obrigatório!")]
    public int Masp { get; set; }

    [Required(ErrorMessage = "Informe uma senha!")]
    [StringLength(150), MaxLength(150, ErrorMessage = "A senha pode conter no máximo 150 caracteres.")]
    [MinLength(8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirme sua senha!")]
    [StringLength(150), MaxLength(150, ErrorMessage = "A senha pode conter no máximo 150 caracteres.")]
    [MinLength(8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres.")]
    public string ComparePassword { get; set; }
    
    [StringLength(35)]
    public string LastModifiedBy { get; set; }
}