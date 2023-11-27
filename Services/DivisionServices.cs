using System.Data.SqlTypes;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class DivisionServices : IDivisionServices
{
    private readonly ILogger<IDivisionServices> _logger;
    private readonly SEP_WebContext _database;

    public DivisionServices(ILogger<IDivisionServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Division> RegisterDivision(Division division)
    {
        division.RegisterDate = DateTime.Now;

        await _database.Divisions.AddAsync(division);
        await _database.SaveChangesAsync();
        
        return division;
    }

    public async Task<Division> DivisionEdit(Division division)
    {
        Division divisionEdit = SearchForId(division.Id) ?? throw new Exception("Houve um erro na atualização do órgão");

        divisionEdit.Name = division.Name;
        divisionEdit.ModifyDate = DateTime.Now;

        divisionEdit.UserAdministratorId = division.UserAdministratorId;
        divisionEdit.InstituitionId = division.InstituitionId;
        divisionEdit.LastModifiedBy = division.LastModifiedBy;

        _database.Divisions.Update(divisionEdit);
        await _database.SaveChangesAsync();

        return divisionEdit;
    }

    public async Task<ICollection<Division>> DivisionsList()
    {
        try
        {
            ICollection<Division> divisions = await _database.Divisions.ToListAsync();

            if (divisions == null)
                throw new ArgumentNullException(nameof(divisions), ExceptionMessages.ErrorArgumentNullException);

                if (divisions?.Count == 0)
                    throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

                    if(_database == null)
                        throw new InvalidOperationException(ExceptionMessages.ErrorDatabaseConnection);

            return divisions ?? new List<Division>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[DIVISION_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[DIVISION_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Division>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<Division>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<Division>();
        }
    }

    public void DeleteDivision(int id)
    {
        Division deleteDivision = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão do órgão");

        _database.Divisions.Remove(deleteDivision);
        _database.SaveChanges();
    }

    public Division SearchForId(int id)
    {
        return _database.Divisions.FirstOrDefault(x => x.Id == id);
    }

    public string DivisionsName(int? divisionId)
    {
        ICollection<Division> division = _database.Divisions.Where(x => x.Id == divisionId).ToList();
        return division.FirstOrDefault().Name;
    }

    public async Task<string> InstituitionName(Division division)
    {
        Instituition instituition = await _database.Instituitions.Where(x => division.InstituitionId == x.Id).FirstOrDefaultAsync();
        return instituition.Name.ToUpper();
    }
}
