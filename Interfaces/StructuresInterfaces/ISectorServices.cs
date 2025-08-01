using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.StructuresInterfaces;

public interface ISectorServices
{
    Sector SearchForId(int id);
    Task<Sector> RegisterSector(Sector sector);
    Task<SectorViewModel> GetByIdAsync(int id);
    IQueryable<Sector> SectorsAsQueryable();
    Task<Sector> SectorEdit(Sector sector);
    Task<string> SectorsName(int? sectorId);
    Task<ICollection<Sector>> GetSectorsAsync(int sectionId);
    void DeleteSector(int id);
}