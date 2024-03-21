using SEP_Web.Models;

namespace SEP_Web.Services;

public interface ISectorServices
{
    Sector SearchForId(int id);
    Task<Sector> RegisterSector(Sector sector);
    Task<ICollection<Sector>> SectorsList();
    Task<Sector> SectorEdit(Sector sector);
    Task<string> SectorsName(int? sectorId);
    Task<ICollection<Sector>> GetSectorsAsync(int sectionId);
    void DeleteSector(int id);
}