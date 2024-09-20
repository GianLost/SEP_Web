using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Interfaces.StructuresInterfaces;
using SEP_Web.ViewModels;

namespace SEP_Web.Services.StructuresRepository;

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

    public async Task<ICollection<DivisionViewModel>> DivisionsList()
    {
        try
        {
            ICollection<Division> divisions = await _database.Divisions.Include(x => x.Instituition).ToListAsync();

            ICollection<DivisionViewModel> divisionViewModels = divisions.Select(x => new DivisionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                InstituitionName = x.Instituition.Name ?? "N/A",
                InstituitionId = x.InstituitionId
            }).ToList();

            if (divisions == null)
                throw new ArgumentNullException(nameof(divisions), ExceptionMessages.ErrorArgumentNullException);

            if (divisions.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            if (_database == null)
                throw new InvalidOperationException(ExceptionMessages.ErrorDatabaseConnection);

            return divisionViewModels ?? new List<DivisionViewModel>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[DIVISION_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[DIVISION_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());
            return new List<DivisionViewModel>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<DivisionViewModel>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<DivisionViewModel>();
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

    public async Task<ICollection<Division>> GetDivisionsAsync(int instituitionId)
    {
        return await _database.Divisions.Where(d => d.InstituitionId == instituitionId).ToListAsync();
    }

    public async Task<string> DivisionsName(int? divisionId)
    {
        ICollection<Division> division = await _database.Divisions.Where(x => x.Id == divisionId).ToListAsync();
        return division.FirstOrDefault().Name;
    }

    public async Task<DivisionViewModel> GetByIdAsync(int id)
    {
        var division = await _database.Divisions
            .Include(s => s.Instituition)  // Inclua quaisquer outras propriedades de navegação necessárias
            .FirstOrDefaultAsync(s => s.Id == id);

        if (division == null)
        {
            throw new KeyNotFoundException($"Seção com ID {id} não encontrada.");
        }

        // Mapeie a entidade para a ViewModel
        var viewModel = new DivisionViewModel
        {
            Id = division.Id,
            Name = division.Name,
            InstituitionName = division.Instituition?.Name,
            InstituitionId = division.InstituitionId
        };

        return viewModel;
    }

    public IQueryable<Division> SectionsAsQueryable()
    {
        return _database.Divisions.Include(x => x.Instituition);
    }
}