using SEP_Web.Models.UsersModels;
using SEP_Web.Models.LicensesModels;
using SEP_Web.Database;
using SEP_Web.Keys;
using SEP_Web.Helper.Messages;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Interfaces.LicensesInterfaces;
using SEP_Web.ViewModels;

namespace SEP_Web.Services.LicensesRepository;

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

        CivilServant user = _database.Servants.Find(servantLicense.CivilServantId);

        if (user != null)
        {
            user.UserStats = UserStatsEnum.UnderLicense;
            _database.Servants.Update(user);
        }

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

    public async Task<ServantLicense> ServantLicensesEdit(ServantLicense servantLicense)
    {
        ServantLicense servantLicensesEdit = SearchForId(servantLicense.Id) ?? throw new Exception("Houve um erro na edição da licença deste servidor !");

        servantLicensesEdit.LicensesId = servantLicense.LicensesId;
        servantLicensesEdit.StartDate = servantLicense.StartDate;
        servantLicensesEdit.EndDate = servantLicense.EndDate;

        servantLicensesEdit.ModifyDate = DateTime.Now;
        servantLicensesEdit.LastModifiedBy = servantLicense.LastModifiedBy;

        _database.ServantLicense.Update(servantLicensesEdit);
        await _database.SaveChangesAsync();

        return servantLicensesEdit;
    }

    public void DeleteServantLicenses(int id)
    {
        ServantLicense deleteServantLicense = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão da licença do servidor");

        // Atualizar o status do usuário para Active
        var user = _database.Servants.FirstOrDefault(u => u.Id == deleteServantLicense.CivilServantId);
        if (user != null)
        {
            user.UserStats = UserStatsEnum.Active;
            _database.Servants.Update(user);
        }

        _database.ServantLicense.Remove(deleteServantLicense);
        _database.SaveChanges();
    }

    public ServantLicense SearchForId(int id)
    {
        return _database.ServantLicense.FirstOrDefault(x => x.Id == id);
    }

    public async Task<ServantLicenseViewModel> GetByIdAsync(int id)
    {
        var servantLicense = await _database.ServantLicense
            .Include(s => s.CivilServant).Include(z => z.License)  // Inclua quaisquer outras propriedades de navegação necessárias
            .FirstOrDefaultAsync(s => s.Id == id) ?? throw new KeyNotFoundException($"Licença para servidor com ID {id} não encontrada.");

        // Mapeie a entidade para a ViewModel
        var viewModel = new ServantLicenseViewModel
        {
            Id = servantLicense.Id,
            Masp = servantLicense.CivilServant.Masp,
            Name = servantLicense.CivilServant.Name,
            CivilServant = servantLicense.CivilServant,
            License = servantLicense.License,
            LicenseName = servantLicense.License.Name,
            StartDate = servantLicense.StartDate.HasValue ? servantLicense.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty, // Alterado para o formato correto
            EndDate = servantLicense.EndDate.HasValue ? servantLicense.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty
        };

        return viewModel;
    }

    public IQueryable<ServantLicense> ServantLicensesAsQueryable()
    {
        return _database.ServantLicense.Include(x => x.CivilServant).Include(z => z.License);
    }
}