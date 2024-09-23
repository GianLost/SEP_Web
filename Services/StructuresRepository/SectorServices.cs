using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Models.StructuresModels;
using SEP_Web.Interfaces.StructuresInterfaces;
using SEP_Web.ViewModels;

namespace SEP_Web.Services.StructuresRepository;

public class SectorServices : ISectorServices
{
    private readonly ILogger<ISectorServices> _logger;
    private readonly SEP_WebContext _database;

    public SectorServices(ILogger<ISectorServices> logger, SEP_WebContext database)
    {
        _logger = logger;
        _database = database;
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
        ICollection<Sector> sector = await _database.Sectors.Where(x => x.Id == sectorId).ToListAsync();
        return sector.FirstOrDefault().Name;
    }

    public async Task<SectorViewModel> GetByIdAsync(int id)
    {
        var sector = await _database.Sectors
            .Include(s => s.Section)  // Inclua quaisquer outras propriedades de navegação necessárias
            .FirstOrDefaultAsync(s => s.Id == id) ?? throw new KeyNotFoundException($"Setor com ID {id} não encontrada.");

        // Mapeie a entidade para a ViewModel
        var viewModel = new SectorViewModel
        {
            Id = sector.Id,
            Name = sector.Name,
            SectionName = sector.Section?.Name,
            SectionId = sector.SectionId
        };

        return viewModel;
    }

    public IQueryable<Sector> SectorsAsQueryable()
    {
        return _database.Sectors.Include(x => x.Section);
    }
}