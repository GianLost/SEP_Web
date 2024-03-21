using System.ComponentModel.DataAnnotations;

namespace SEP_Web.Models;

public class Login
{
    [Required(ErrorMessage = "Informe seu MASP !")]
    public int? Masp { get; set; }

    [Required(ErrorMessage = "Informe seu login !")]
    public string LoginName { get; set; }

    [Required(ErrorMessage = "Informe sua senha !")]
    public string Password { get; set; }
}