using SEP_Web.Models;

namespace SEP_Web.Services;

public interface IUserEvaluatorServices
{
    Task<UserEvaluator> RegisterEvaluator(UserEvaluator user); // Gera um novo registro de usuário administrador;
    Task<ICollection<UserEvaluator>> EvaluatorsList(); // Coleção de administradores;
    Task<UserEvaluator> EvaluatorsEdit(UserEvaluator user); // Edição de um registro já existente de um usuário administrador;
    void DeleteEvaluator(int id); // Exclui usuário administrador;
    UserEvaluator SearchForId(int id); // Busca um usuário pelo Id;
}