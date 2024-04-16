using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Interfaces.UsersInterfaces;

public interface ICivilServantServices 
{ 
    Task<CivilServant> RegisterServant(CivilServant user); // Gera um novo registro de usuário administrador;
    Task<ICollection<CivilServant>> ServantsList(); // Coleção de administradores;
    Task<ICollection<CivilServant>> ServantsListUnderLicense();
    Task<CivilServant> ServantsEdit(CivilServant user); // Edição de um registro já existente de um usuário administrador;
    Task<CivilServant> EditStructures(ModifyServantStructures modifyStructures);
    void DeleteServant(int id); // Exclui usuário administrador;
    CivilServant SearchForId(int id); // Busca um usuário pelo Id;
}
