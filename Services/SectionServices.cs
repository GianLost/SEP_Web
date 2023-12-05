using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class SectionServices : ISectionServices
{
    private readonly ILogger<ISectionServices> _logger;
    private readonly SEP_WebContext _database;

    public SectionServices(ILogger<ISectionServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<Section> RegisterSection(Section section)
    {
        section.RegisterDate = DateTime.Now;

        await _database.Sections.AddAsync(section);
        await _database.SaveChangesAsync();

        return section;
    }

    public async Task<Section> SectionEdit(Section section)
    {
        Section sectionEdit = SearchForId(section.Id) ?? throw new Exception("Houve um erro na atualização da sessão");

        sectionEdit.Name = section.Name;
        sectionEdit.ModifyDate = DateTime.Now;

        sectionEdit.UserAdministratorId = section.UserAdministratorId;
        sectionEdit.DivisionId = section.DivisionId;
        sectionEdit.LastModifiedBy = section.LastModifiedBy;

        _database.Sections.Update(sectionEdit);
        await _database.SaveChangesAsync();

        return sectionEdit;
    }

    public async Task<ICollection<Section>> SectionsList()
    {
        try
        {
            ICollection<Section> sections = await _database.Sections.ToListAsync();

            if (sections == null)
                throw new ArgumentNullException(nameof(sections), ExceptionMessages.ErrorArgumentNullException);

            if (sections?.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            if (_database == null)
                throw new InvalidOperationException(ExceptionMessages.ErrorDatabaseConnection);

            return sections ?? new List<Section>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SECTION_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SECTION_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Section>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<Section>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<Section>();
        }
    }

    public void DeleteSection(int id)
    {
        Section deleteSection = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão da sessão");

        _database.Sections.Remove(deleteSection);
        _database.SaveChanges();
    }

    public Section SearchForId(int id)
    {
        return _database.Sections.FirstOrDefault(x => x.Id == id);
    }

    public string SectionsName(int? sectionId)
    {
        ICollection<Section> section = _database.Sections.Where(x => x.Id == sectionId).ToList();
        return section.FirstOrDefault().Name;
    }

    public async Task<string> DivisionName(Section section)
    {
        Division division = await _database.Divisions.Where(x => section.DivisionId == x.Id).FirstOrDefaultAsync();
        return division.Name.ToUpper();
    }

    public async Task<ICollection<Section>> GetSectionsAsync(int divisionId)
    {
        return await _database.Sections.Where(s => s.DivisionId == divisionId).ToListAsync();
    }
}
