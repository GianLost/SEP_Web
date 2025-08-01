using SEP_Web.Models.UsersModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.UsersInterfaces;

public interface IUserAdministratorServices
{
    Task<UserAdministrator> RegisterAdministrator(UserAdministrator user); // Gera um novo registro de usuário administrador;
    Task<ICollection<UserAdministrator>> AdministratorsList(); // Coleção de administradores;
    Task<UsersViewModel> GetByIdAsync(int id);
    IQueryable<UserAdministrator> AdministratorsAsQueryable();
    Task<UserAdministrator> AdministratorsEdit(UserAdministrator user); // Edição de um registro já existente de um usuário administrador;
    void DeleteAdministrator(int id); // Exclui usuário administrador;
    Task<string> AdministratorsName(int? AdministratorsId);
    UserAdministrator SearchForId(int id); // Busca um usuário pelo Id;
}