using Microsoft.AspNetCore.Mvc;
using SEP_Web.Models;

namespace SEP_Web.Helper.Authentication;
public interface IUserSession
{
    void UserCheckIn(Users users); // Recebe como parâmetro um objeto de "Users". É o método responsável por passar as Json string serializadas para que se estabeleça uma sessão;
    void UserCheckOut(); // Remove os dados serializados limpando e finalizando a sessão;
    Task<Users> SearchUserSession(); // Busca por uma variável de sessão a fim de verificar se já existe uma sessão estabelecida;
    Task<Users> UserSignIn(int? masp, string login, Controller controller); // Recebe como parâmetros um inteiro que corresponde a um índice de uma classe de usuários, e uma string correspondente ao nome de login , ambos serão utilizados para validar a existência e autenticidade de um usuário permitido a estabelecer uma determinada sessão. Fornece à UserCheckIn o objeto que será serializado após validá-lo;
}