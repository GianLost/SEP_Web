using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class ServantLicenseServices : IServantLicenseServices
{
    private readonly ILogger<IServantLicenseServices> _logger;
    private readonly SEP_WebContext _database;

    public ServantLicenseServices(ILogger<IServantLicenseServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<ServantLicense> RegisterServantLicense(ServantLicense servantLicense)
    {
        servantLicense.RegisterDate = DateTime.Now;

        await _database.ServantLicense.AddAsync(servantLicense);
        await _database.SaveChangesAsync();

        return servantLicense;
    }

    public async Task<ICollection<ServantLicense>> ServantLicenseList()
    {
       try
        {
            ICollection<ServantLicense> servantLicenses = await _database.ServantLicense.ToListAsync();

            if (servantLicenses?.Count == 0)
                throw new ArgumentNullException(nameof(servantLicenses), ExceptionMessages.ErrorArgumentNullException);

            return servantLicenses ?? new List<ServantLicense>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[LICENSE_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[LICENSE_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<ServantLicense>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<ServantLicense>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<ServantLicense>();
        }
    }

    public Task<ServantLicense> ServantLicensesEdit(ServantLicense servantLicense)
    {
        throw new NotImplementedException();
    }

    public void DeleteServantLicenses(int id)
    {
        ServantLicense deleteServantLicense = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão da licença do servidor");

        _database.ServantLicense.Remove(deleteServantLicense);
        _database.SaveChanges();
    }

    public ServantLicense SearchForId(int id)
    {
       return _database.ServantLicense.FirstOrDefault(x => x.Id == id);
    }
}