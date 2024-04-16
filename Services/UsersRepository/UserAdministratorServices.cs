using SEP_Web.Models.UsersModels;
using SEP_Web.Interfaces.UsersInterfaces;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Database;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;



namespace SEP_Web.Services.UsersRepository;

public class UserAdministratorServices : IUserAdministratorServices
{
    private readonly ILogger<IUserAdministratorServices> _logger;
    private readonly SEP_WebContext _database;

    public UserAdministratorServices(ILogger<IUserAdministratorServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<UserAdministrator> RegisterAdministrator(UserAdministrator user)
    {
        try
        {
            if (user == null) throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            user.UserType = Keys.UsersTypeEnum.User_Admin;
            user.UserStats = Keys.UserStatsEnum.Active;
            user.Password = Cryptography.EncryptPassword(user.Password);
            user.RegisterDate = DateTime.Now;

            await _database.Administrators.AddAsync(user);
            await _database.SaveChangesAsync();

            return user;

        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);

            _logger.LogError("[ADMINISTRATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            user = null;
            return user;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[ADMINISTRATOR_SERVICE]: {Description}", nullException.StackTrace.Trim());

            user = null;
            return user;
        }
    }

    public async Task<ICollection<UserAdministrator>> AdministratorsList()
    {
        try
        {
            ICollection<UserAdministrator> administrators = await _database.Administrators.ToListAsync();

            if (administrators?.Count == 0)
                throw new ArgumentNullException(nameof(administrators), ExceptionMessages.ErrorArgumentNullException);

            return administrators ?? new List<UserAdministrator>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ADMINISTRATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<UserAdministrator>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<UserAdministrator>();
        }
    }

    public async Task<UserAdministrator> AdministratorsEdit(UserAdministrator user)
    {
        try
        {
            UserAdministrator userEdit = SearchForId(user.Id) ?? throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            userEdit.Masp = user.Masp;
            userEdit.Name = user.Name;
            userEdit.Login = user.Login;
            userEdit.Email = user.Email;
            userEdit.Phone = user.Phone;
            userEdit.Position = user.Position;
            userEdit.UserStats = user.UserStats;
            userEdit.LastModifiedBy = user.LastModifiedBy;
            userEdit.ModifyDate = DateTime.Now;

            _database.Administrators.Update(userEdit);
            await _database.SaveChangesAsync();

            return userEdit;
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);

            _logger.LogError("[ADMINISTRATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            user = null;
            return user;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);

            _logger.LogWarning("[ADMINISTRATOR_SERVICE]: {Description}", nullException.StackTrace.Trim());

            user = null;
            return user;
        }
    }

    public void DeleteAdministrator(int id)
    {
        try
        {
            UserAdministrator searchUser = SearchForId(id) ?? throw new ArgumentNullException(nameof(searchUser), ExceptionMessages.ErrorArgumentNullException);

            _database.Administrators.Remove(searchUser);
            _database.SaveChanges();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[ADMINISTRATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[ADMINISTRATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());
        }
    }

    public async Task<string> AdministratorsName(int? AdministratorsId)
    {
        ICollection<UserAdministrator> administrators =  await _database.Administrators.Where(x => x.Id == AdministratorsId).ToListAsync();
        return administrators.FirstOrDefault().Name;
    }

    public UserAdministrator SearchForId(int id)
    {
        return _database.Administrators.FirstOrDefault(x => x.Id == id);
    }
}