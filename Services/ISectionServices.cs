using SEP_Web.Models;

namespace SEP_Web.Services;

public interface ISectionServices
{
    Section SearchForId(int id);
    Task<Section> RegisterSection(Section section);
    Task<ICollection<Section>> SectionsList();
    Task<Section> SectionEdit(Section section);
    Task<string> SectionsName(int? sectionId);
    Task<ICollection<Section>> GetSectionsAsync(int divisionId);
    void DeleteSection(int id);
}