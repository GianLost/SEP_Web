using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Models;

namespace SEP_Web.Services;
public class SectorServices : ISectorServices
{
    private readonly SEP_WebContext _database;

    public SectorServices(SEP_WebContext database)
    {
        _database = database;
    }

    public async Task<ICollection<Sector>> SectorsList()
    {
        ICollection<Sector> sectors = await _database.Sectors.ToListAsync();
        return sectors;
    }

    public async Task<Sector> RegisterSector(Sector sector)
    {
        sector.RegisterDate = DateTime.Now;

        await _database.Sectors.AddAsync(sector);
        await _database.SaveChangesAsync();

        return sector;
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

    public Task<Sector> SectorEdit(Sector sector)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SectionName(Sector sector)
    {
       Section sections = await _database.Sections.Where(x => sector.SectionId == x.Id).FirstOrDefaultAsync();
       return sections.Name.ToUpper();
    }
}
