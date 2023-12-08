using SEP_Web.Database;
using SEP_Web.Helper.Authentication;
using SEP_Web.Keys;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class UsersServices : IUsersServices
{
    private readonly ILogger<UsersServices> _logger;
    private readonly SEP_WebContext _database;

    public UsersServices(ILogger<UsersServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Users> ChangePassword(ChangePassword changePassword)
    {
        Users searchUser = SearchUserToChangePass(changePassword.Id, changePassword.Masp) ?? throw new Exception("Houve um erro na atualização da senha, usuário não foi encontrado ou os dados informados são inválidos!");

        searchUser.Password = Cryptography.EncryptPassword(changePassword.Password);
        searchUser.ModifyDate = DateTime.Now;

        if (searchUser.UserType == UsersTypeEnum.User_Admin)
        {
            _database.Administrators.Update((UserAdministrator)searchUser);
        }
        else if (searchUser.UserType == UsersTypeEnum.User_Evaluator)
        {
            _database.Evaluators.Update((UserEvaluator)searchUser);
        }
        else if (searchUser.UserType == UsersTypeEnum.User_Server)
        {
            _database.Servants.Update((CivilServant)searchUser);
        }
        else
        {
            throw new Exception("Tipo de usuário inválido.");
        }

        await _database.SaveChangesAsync();
        return searchUser;
    }

    public Users SearchUserToChangePass(int id, int masp)
    {
        Users users = _database.Administrators.FirstOrDefault(x => x.Id == id && x.Masp == masp);

        users ??= _database.Evaluators.FirstOrDefault(x => x.Id == id && x.Masp == masp);

        users ??= _database.Servants.FirstOrDefault(x => x.Id == id && x.Masp == masp);

        if(!(users == null))
        {
            return users;
        }

        throw new ArgumentNullException(nameof(users), $"Usuário não encontrado, um ou mais argumentos retornaram NULL");
    }
}
