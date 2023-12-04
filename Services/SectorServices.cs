using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class SectorServices : ISectorServices
{
    private readonly ILogger<ISectorServices> _logger;
    private readonly SEP_WebContext _database;

    public SectorServices(ILogger<ISectorServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<ICollection<Sector>> SectorsList()
    {
        try
        {
            ICollection<Sector> sectors = await _database.Sectors.ToListAsync();
            if (sectors == null)
                    throw new ArgumentNullException(nameof(sectors), ExceptionMessages.ErrorArgumentNullException);

                    if (sectors?.Count == 0)
                        throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

                        if(_database == null)
                            throw new InvalidOperationException(ExceptionMessages.ErrorDatabaseConnection);

            return sectors ?? new List<Sector>();
        }
        catch (MySqlException mySqlException)
        {
            // MYSQL EXEPTIONS :

            _logger.LogError("[SECTOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SECTOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<Sector>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<Sector>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<Sector>();
        }
    }

    public async Task<Sector> RegisterSector(Sector sector)
    {
        sector.RegisterDate = DateTime.Now;

        await _database.Sectors.AddAsync(sector);
        await _database.SaveChangesAsync();

        return sector;
    }

    public async Task<Sector> SectorEdit(Sector sector)
    {
        Sector sectorEdit = SearchForId(sector.Id) ?? throw new Exception("Houve um erro na atualização da setor");

        sectorEdit.Name = sector.Name;
        sectorEdit.ModifyDate = DateTime.Now;

        sectorEdit.UserAdministratorId = sector.UserAdministratorId;
        sectorEdit.SectionId = sector.SectionId;
        sectorEdit.LastModifiedBy = sector.LastModifiedBy;

        _database.Sectors.Update(sectorEdit);
        await _database.SaveChangesAsync();

        return sectorEdit;
    }

    public void DeleteSector(int id)
    {
        Sector deleteSector = SearchForId(id) ?? throw new Exception("Houve um erro na exclusão do setor");

        _database.Sectors.Remove(deleteSector);
        _database.SaveChanges();
    }

    public Sector SearchForId(int id)
    {
        return _database.Sectors.FirstOrDefault(x => x.Id == id);
    }

    public async Task<string> SectionName(Sector sector)
    {
       Section sections = await _database.Sections.Where(x => sector.SectionId == x.Id).FirstOrDefaultAsync();
       return sections.Name.ToUpper();
    }
}
