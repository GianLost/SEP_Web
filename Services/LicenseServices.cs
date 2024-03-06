using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class LicenseServices : ILicenseServices
{
    private readonly ILogger<ILicenseServices> _logger;
    private readonly SEP_WebContext _database;

    public LicenseServices(ILogger<ILicenseServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Licenses> RegisterLicense(Licenses license)
    {
        license.RegisterDate = DateTime.Now;

        await _database.Licenses.AddAsync(license);
        await _database.SaveChangesAsync();

        return license;
    }

    public async Task<ICollection<Licenses>> LicenseList()
    {
        try
        {
            ICollection<Licenses> licenses = await _database.Licenses.ToListAsync();

            if (licenses?.Count == 0)
                throw new ArgumentNullException(nameof(licenses), ExceptionMessages.ErrorArgumentNullException);

            return licenses ?? new List<Licenses>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[LICENSE_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[LICENSE_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Licenses>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<Licenses>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<Licenses>();
        }
    }

    public async Task<Licenses> LicensesEdit(Licenses license)
    {
        Licenses licensesEdit = SearchForId(license.Id) ?? throw new Exception("Houve um erro na edição da licença !");

        licensesEdit.Name = license.Name;
        licensesEdit.Time = license.Time;

        licensesEdit.ModifyDate = DateTime.Now;

        licensesEdit.UserAdministratorId = license.UserAdministratorId;
        licensesEdit.LastModifiedBy = license.LastModifiedBy;

        _database.Licenses.Update(licensesEdit);
        await _database.SaveChangesAsync();

        return licensesEdit;
    }

    public void DeleteLicenses(int id)
    {
        Licenses deleteLicense = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão da licença");

        _database.Licenses.Remove(deleteLicense);
        _database.SaveChanges();
    }

    public Licenses SearchForId(int id)
    {
        return _database.Licenses.FirstOrDefault(x => x.Id == id);
    }

}