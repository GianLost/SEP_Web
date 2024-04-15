using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Keys;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Services;

public class CivilServantServices : ICivilServantServices
{
    private readonly ILogger<ICivilServantServices> _logger;
    private readonly SEP_WebContext _database;

    public CivilServantServices(ILogger<ICivilServantServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<CivilServant> RegisterServant(CivilServant user)
    {
        try
        {
            if (user == null) throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            user.UserType = UsersTypeEnum.User_Server;
            user.UserStats = UserStatsEnum.Active;
            user.Password = Cryptography.EncryptPassword(user.Password);
            user.RegisterDate = DateTime.Now;

            await _database.Servants.AddAsync(user);
            await _database.SaveChangesAsync();

            return user;

        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SERVANT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SERVANT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            user = null;
            return user;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[SERVANT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[SERVANT_SERVICE]: {Description}", nullException.StackTrace.Trim());

            user = null;
            return user;
        }
    }

    public async Task<ICollection<CivilServant>> ServantsList()
    {
        try
        {
            ICollection<CivilServant> servants = await _database.Servants.ToListAsync();

            if (servants?.Count == 0)
                throw new ArgumentNullException(nameof(servants), ExceptionMessages.ErrorArgumentNullException);

            return servants ?? new List<CivilServant>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SERVANT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SERVANT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<CivilServant>();
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[SERVANT_SERVICE]: {exceptionMessage} : {Message}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            _logger.LogWarning("[SERVANT_SERVICE]: {Description}", ex.StackTrace.Trim());

            return new List<CivilServant>();
        }
    }

    public async Task<ICollection<CivilServant>> ServantsListUnderLicense()
    {
        try
        {
            ICollection<CivilServant> servants = await _database.Servants.Where(x => x.UserStats != UserStatsEnum.UnderLicense).ToListAsync();

            if (servants?.Count == 0)
                throw new ArgumentNullException(nameof(servants), ExceptionMessages.ErrorArgumentNullException);

            return servants ?? new List<CivilServant>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SERVANT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SERVANT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<CivilServant>();
        }
        catch (ArgumentNullException ex)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[SERVANT_SERVICE]: {exceptionMessage} : {Message}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, ex.Message, ex.InnerException);
            _logger.LogWarning("[SERVANT_SERVICE]: {Description}", ex.StackTrace.Trim());

            return new List<CivilServant>();
        }
    }

    public async Task<CivilServant> ServantsEdit(CivilServant user)
    {
        try
        {
            CivilServant userEdit = SearchForId(user.Id) ?? throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            userEdit.Masp = user.Masp;
            userEdit.Name = user.Name;
            userEdit.Login = user.Login;
            userEdit.Email = user.Email;
            userEdit.Phone = user.Phone;
            userEdit.Position = user.Position;
            userEdit.UserStats = user.UserStats;
            userEdit.LastModifiedBy = user.LastModifiedBy;
            userEdit.ModifyDate = DateTime.Now;

            _database.Servants.Update(userEdit);
            await _database.SaveChangesAsync();

            return userEdit;
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[EVALUATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[EVALUATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            user = null;
            return user;
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[EVALUATOR_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[EVALUATOR_SERVICE]: {Description}", nullException.StackTrace.Trim());

            user = null;
            return user;
        }
    }

    public async Task<CivilServant> EditStructures(ModifyServantStructures modifyStructures)
    {
        CivilServant searchEvaluator = SearchForId(modifyStructures.Id) ?? throw new ArgumentNullException(nameof(modifyStructures), "Houve um erro ao buscar o avaliador para editar as estruturas !");

        searchEvaluator.InstituitionId = modifyStructures.InstituitionId;
        searchEvaluator.DivisionId = modifyStructures.DivisionId;
        searchEvaluator.SectionId = modifyStructures.SectionId;
        searchEvaluator.SectorId = modifyStructures.SectorId;
        searchEvaluator.UserEvaluatorId1 = modifyStructures.UserEvaluatorId1;
        searchEvaluator.UserEvaluatorId2 = modifyStructures.UserEvaluatorId2;
        searchEvaluator.ModifyDate = DateTime.Now;

        _database.Servants.Update(searchEvaluator);
        await _database.SaveChangesAsync();

        return searchEvaluator;
    }

    public void DeleteServant(int id)
    {
        try
        {
            CivilServant searchUser = SearchForId(id) ?? throw new ArgumentNullException(nameof(searchUser), ExceptionMessages.ErrorArgumentNullException);

            _database.Servants.Remove(searchUser);
            _database.SaveChanges();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SERVANT_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SERVANT_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[SERVANT_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[SERVANT_SERVICE]: {Description}", nullException.StackTrace.Trim());
        }
    }

    public CivilServant SearchForId(int id)
    {
        return _database.Servants.FirstOrDefault(x => x.Id == id);
    }

}
