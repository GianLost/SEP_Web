using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Authentication;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;

public class UserEvaluatorServices : IUserEvaluatorServices
{
    private readonly ILogger<IUserEvaluatorServices> _logger;
    private readonly SEP_WebContext _database;

    public UserEvaluatorServices(ILogger<IUserEvaluatorServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<UserEvaluator> RegisterEvaluator(UserEvaluator user)
    {
        try
        {
            if (user == null) throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            user.UserType = Keys.UsersTypeEnum.User_Evaluator;
            user.UserStats = Keys.UserStatsEnum.Active;
            user.Password = Cryptography.EncryptPassword(user.Password);
            user.RegisterDate = DateTime.Now;

            await _database.Evaluators.AddAsync(user);
            await _database.SaveChangesAsync();

            return user;

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

    public async Task<ICollection<UserEvaluator>> EvaluatorsList()
    {
        try
        {
            ICollection<UserEvaluator> evaluators = await _database.Evaluators.ToListAsync();

            if (evaluators?.Count == 0)
                throw new ArgumentNullException(nameof(evaluators), ExceptionMessages.ErrorArgumentNullException);

            return evaluators ?? new List<UserEvaluator>();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[EVALUATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[EVALUATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<UserEvaluator>();
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[EVALUATOR_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[EVALUATOR_SERVICE]: {Description}", nullException.StackTrace.Trim());

            return new List<UserEvaluator>();
        }
    }

    public async Task<UserEvaluator> EvaluatorsEdit(UserEvaluator user)
    {
        try
        {
            UserEvaluator userEdit = SearchForId(user.Id) ?? throw new ArgumentNullException(nameof(user), ExceptionMessages.ErrorArgumentNullException);

            userEdit.Masp = user.Masp;
            userEdit.Name = user.Name;
            userEdit.Login = user.Login;
            userEdit.Email = user.Email;
            userEdit.Phone = user.Phone;
            userEdit.Position = user.Position;
            userEdit.UserStats = user.UserStats;
            userEdit.LastModifiedBy = user.LastModifiedBy;
            userEdit.ModifyDate = DateTime.Now;

            _database.Evaluators.Update(userEdit);
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

    public void DeleteEvaluator(int id)
    {
        try
        {
            UserEvaluator searchUser = SearchForId(id) ?? throw new ArgumentNullException(nameof(searchUser), ExceptionMessages.ErrorArgumentNullException);

            _database.Evaluators.Remove(searchUser);
            _database.SaveChanges();
        }
        catch (DbUpdateException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[EVALUATOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[EVALUATOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());
        }
        catch (Exception ex) when (ex.InnerException is ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("[EVALUATOR_SERVICE]: {exceptionMessage} : {Message}, Attribute = {ParamName}, value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.ParamName, nullException.InnerException);
            _logger.LogWarning("[EVALUATOR_SERVICE]: {Description}", nullException.StackTrace.Trim());
        }
    }

    public async Task<UserEvaluator> EditStructures(ModifyStructures modifyStructures)
    {
        UserEvaluator searchEvaluator = SearchForId(modifyStructures.Id) ?? throw new ArgumentNullException(nameof(modifyStructures), "Houve um erro ao buscar o avaliador para editar as estruturas !");

        searchEvaluator.InstituitionId = modifyStructures.InstituitionId;
        searchEvaluator.DivisionId = modifyStructures.DivisionId;
        searchEvaluator.SectionId = modifyStructures.SectionId;
        searchEvaluator.SectorId = modifyStructures.SectorId;
        searchEvaluator.ModifyDate = DateTime.Now;

        _database.Evaluators.Update(searchEvaluator);
        await _database.SaveChangesAsync();

        return searchEvaluator;
    }
    public async Task<ICollection<UserEvaluator>> GetFirstEvaluatorForRelationToServantAsync(int instituitionId)
    {
        return await _database.Evaluators.Where(d => d.InstituitionId == instituitionId).ToListAsync();
    }

    public async Task<ICollection<UserEvaluator>> GetSecondEvaluatorForRelationToServantAsync(int firstEvaluator, int instituitionId)
    {
        return await _database.Evaluators.Where(d =>  d.Id != firstEvaluator && d.InstituitionId == instituitionId).ToListAsync();
    }

    public async Task<string> EvaluatorsName(int? EvaluatorsId)
    {
        ICollection<UserEvaluator> evaluators =  await _database.Evaluators.Where(x => x.Id == EvaluatorsId).ToListAsync();
        return evaluators.FirstOrDefault().Name;
    }

    public UserEvaluator SearchForId(int id)
    {
        return _database.Evaluators.FirstOrDefault(x => x.Id == id);
    }
}