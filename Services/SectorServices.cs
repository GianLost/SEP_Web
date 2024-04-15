using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

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

    public async Task<ICollection<SectorViewModel>> SectorsList()
    {
        try
        {
            ICollection<Sector> sectors = await _database.Sectors.Include(x => x.Section).ToListAsync();

            ICollection<SectorViewModel> sectorViewModels = sectors.Select(x => new SectorViewModel
            {
                Id = x.Id,
                Name = x.Name,
                SectionName = x.Section.Name ?? "N/A",
                SectionId = x.SectionId
            }).ToList();

            if (sectors.Count == 0)
                throw new TargetParameterCountException(FeedbackMessages.ErrorEmptyCollection);

            return sectorViewModels ?? new List<SectorViewModel>();
        }
        catch (MySqlException mySqlException)
        {
            _logger.LogError("[SECTOR_SERVICE]: {exceptionMessage} : , {Message}, ErrorCode = {errorCode} - Represents {Error} ", ExceptionMessages.ErrorDatabaseConnection, mySqlException.Message.ToUpper(), mySqlException.Number, mySqlException.ErrorCode);
            _logger.LogError("[SECTOR_SERVICE] : Detalhamento dos erros: {Description} - ", mySqlException.StackTrace.Trim());

            return new List<SectorViewModel>();
        }
        catch (ArgumentNullException nullException)
        {
            // NULL EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", ExceptionMessages.ErrorArgumentNullException, nullException.Message, nullException.InnerException);
            _logger.LogWarning("{Description}", nullException.StackTrace.Trim());

            return new List<SectorViewModel>();
        }
        catch (TargetParameterCountException emptyException)
        {
            // EMPTY EXCEPTION :

            _logger.LogWarning("{exceptionMessage} : {Message} value = '{InnerExeption}'", FeedbackMessages.ErrorEmptyCollection, emptyException.Message, emptyException.InnerException);
            _logger.LogWarning("{Description}", emptyException.StackTrace.Trim());

            return new List<SectorViewModel>();
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

    public async Task<ICollection<Sector>> GetSectorsAsync(int sectionId)
    {
        return await _database.Sectors.Where(s => s.SectionId == sectionId).ToListAsync();
    }

    public async Task<string> SectorsName(int? sectorId)
    {
        ICollection<Sector> sector =  await _database.Sectors.Where(x => x.Id == sectorId).ToListAsync();
        return sector.FirstOrDefault().Name;
    }
}