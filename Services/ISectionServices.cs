using SEP_Web.Models;

namespace SEP_Web.Services;
public interface ISectionServices
{
    Section SearchForId(int id);
    Task<Section> RegisterSection(Section section);
    Task<ICollection<Section>> SectionsList();
    string SectionsName(int? sectionId);
    Task<Section> SectionEdit(Section section);
    Task<string> DivisionName(Section sectionId);
    Task<ICollection<Section>> GetSectionsAsync(int divisionId);
    void DeleteSection(int id);
}
