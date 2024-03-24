using SEP_Web.Models;
using SEP_Web.ViewModels;

namespace SEP_Web.Services;

public interface ISectorServices
{
    Sector SearchForId(int id);
    Task<Sector> RegisterSector(Sector sector);
    Task<ICollection<SectorViewModel>> SectorsList();
    Task<Sector> SectorEdit(Sector sector);
    Task<string> SectorsName(int? sectorId);
    Task<ICollection<Sector>> GetSectorsAsync(int sectionId);
    void DeleteSector(int id);
}