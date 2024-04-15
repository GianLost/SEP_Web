using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Services;
public class InstituitionServices : IInstituitionServices
{
    private readonly ILogger<IInstituitionServices> _logger;
    private readonly SEP_WebContext _database;

    public InstituitionServices(ILogger<IInstituitionServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Instituition> RegisterInstituition(Instituition instituition)
    {
        instituition.RegisterDate = DateTime.Now;

        await _database.Instituitions.AddAsync(instituition);
        await _database.SaveChangesAsync();

        return instituition;
    }

    public async Task<ICollection<InstituitionViewModel>> InstituitionsList()
    {
        try
        {
            ICollection<Instituition> instituition = await _database.Instituitions.Include(s => s.UserAdministrator).ToListAsync();

            ICollection<InstituitionViewModel> instituitionViewModels = instituition.Select(s => new InstituitionViewModel
            {
                Id = s.Id,
                Name = s.Name,
                UserAdministratorId = s.UserAdministratorId
            }).ToList();

            if (instituition == null)
                throw new ArgumentNullException(nameof(instituition), ExceptionMessages.ErrorArgumentNullException);

            if (instituition?.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            if (_database == null)
                throw new InvalidOperationException(ExceptionMessages.ErrorDatabaseConnection);

            return instituitionViewModels ?? new List<InstituitionViewModel>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[INSTITUITION_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[INSTITUITION_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<InstituitionViewModel>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<InstituitionViewModel>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<InstituitionViewModel>();
        }

    }

    public async Task<Instituition> InstituitionEdit(Instituition instituition)
    {
        Instituition instituitionEdit = SearchForId(instituition.Id) ?? throw new Exception("Houve um erro na atualização do órgão");

        instituitionEdit.Name = instituition.Name;
        instituitionEdit.ModifyDate = DateTime.Now;

        instituitionEdit.UserAdministratorId = instituition.UserAdministratorId;
        instituitionEdit.LastModifiedBy = instituition.LastModifiedBy;

        _database.Instituitions.Update(instituitionEdit);
        await _database.SaveChangesAsync();

        return instituitionEdit;
    }

    public async Task<string> InstituitionsName(int? instituitionId)
    {
        ICollection<Instituition> instituition =  await _database.Instituitions.Where(x => x.Id == instituitionId).ToListAsync();
        return instituition.FirstOrDefault().Name;
    }

    public void DeleteInstituition(int id)
    {
        Instituition deleteInstituition = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão do órgão");

        _database.Instituitions.Remove(deleteInstituition);
        _database.SaveChanges();
    }

    public Instituition SearchForId(int id)
    {
        return _database.Instituitions.FirstOrDefault(x => x.Id == id);
    }
}