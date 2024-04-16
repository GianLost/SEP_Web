using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.StructuresInterfaces;

public interface ISectionServices
{
    Section SearchForId(int id);
    Task<Section> RegisterSection(Section section);
    Task<ICollection<SectionViewModel>> SectionsList();
    Task<Section> SectionEdit(Section section);
    Task<string> SectionsName(int? sectionId);
    Task<ICollection<Section>> GetSectionsAsync(int divisionId);
    void DeleteSection(int id);
}