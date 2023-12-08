using SEP_Web.Models;

namespace SEP_Web.Services;
public interface IUsersServices
{
    Users SearchUserToChangePass(int id, int masp); // Busca um usuário pelo Id e MASP;
    Task<Users> ChangePassword(ChangePassword changePassword); // Alterar senha de um usuário;
}