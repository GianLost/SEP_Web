using SEP_Web.Models;

namespace SEP_Web.Services;
public interface ISectorServices
{
    Sector SearchForId(int id);
    Task<Sector> RegisterSector(Sector sector);
    Task<ICollection<Sector>> SectorsList();
    Task<string> SectionsName(Sector sectorId);
    Task<Sector> SectorEdit(Sector sector);
    void DeleteSector(int id);
}
