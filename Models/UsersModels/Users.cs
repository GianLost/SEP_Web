using System.ComponentModel.DataAnnotations;
using SEP_Web.Keys;

namespace SEP_Web.Models.UsersModels;

/// <summary>
/// 
///     <para>Classe pai que representa os diversos tipos de usuário presentes no sistema.
///     A classe <b>Users</b> contém atributos em comum que podem ser herdados pelos diversos níveis de usuário.</para>
///     
///     <para><b>Níveis de usuário:</b></para> 
///     
///     <para><b>- Usuário Administrador:</b></para>
///      
///     <para>Representa o nível mais avançado de um utilizador do sistema, possuindo todos os privilégios
///     de acesso e modificação sobre os demais usuários e estruturas presentes no sistema.</para>
///     
///     <para><b>- Usuário Avaliador:</b></para> 
///     
///     <para>Representa um nível inferior ao usuário administrador. O avaliador possui permissões
///     de acesso e modificação apenas sobre si mesmo e sobre os servidores públicos, que representam o nível mais básico de usuário.</para>
///     
///     <para><b>- Servidores Públicos:</b></para>
///     
///     <para>Representa o nível de usuário mais inferior do sistema. Os servidores públicos podem apenas 
///     ser registrados no sistema para que possam ser gerenciados pelos usuários superiores e possuem acesso apenas ao seu
///     relatório de estágio.</para>
///     
/// </summary>

public class Users
{
    [Key, Required(ErrorMessage = "O campo id é obrigatório !")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo MASP é obrigatório !")]
    [Range(111, 999999, ErrorMessage = "Informe um MASP válido.")]
    public int? Masp { get; set; }

    [Required(ErrorMessage = "O campo nome é obrigatório !")]
    [StringLength(50), MaxLength(50, ErrorMessage = "O nome não pode conter mais de 50 caractéres.")]
    [MinLength(3, ErrorMessage = "O nome não pode conter menos de 3 caractéres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Informe um nome de login !")]
    [StringLength(35), MaxLength(35, ErrorMessage = "O login não pode conter mais de 35 caractéres.")]
    [MinLength(5, ErrorMessage = "O login não pode conter menos de 5 caractéres.")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Informe uma senha !")]
    [StringLength(150), MaxLength(150, ErrorMessage = "A senha pode conter no máximo 150 caracteres.")]
    [MinLength(8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres.")]
    public string Password { get; set; }

    [StringLength(100), Required(ErrorMessage = "Informe um e-mail !")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
    public string Email { get; set; }

    [StringLength(15), Required(ErrorMessage = "Informe um telefone !")]
    [RegularExpression(@"^\([1-9]{2}\) (?:[2-8]|9[0-9])[0-9]{3}\-[0-9]{4}$", ErrorMessage = "O número de telefone é inválido. Tente:(XX) XXXXX-XXXX")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Informe seu cargo !")]
    [StringLength(35), MaxLength(35, ErrorMessage = "O cargo não pode conter mais de 35 caractéres.")]
    [MinLength(3, ErrorMessage = "O cargo não pode conter menos de 3 caractéres.")]
    public string Position { get; set; }

    [Required(ErrorMessage = "Informe o tipo de usuário !")]
    public UsersTypeEnum UserType { get; set; }

    [Required(ErrorMessage = "Informe o status de usuário !")]
    public UserStatsEnum UserStats { get; set; }

    [Required(ErrorMessage = "informe uma data de registro !")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime RegisterDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? ModifyDate { get; set; }

    [StringLength(35)]
    public string LastModifiedBy { get; set; }
}