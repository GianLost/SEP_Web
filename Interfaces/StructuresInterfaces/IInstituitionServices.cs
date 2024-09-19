using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.StructuresInterfaces;

public interface IInstituitionServices
{
    Instituition SearchForId(int id);
    Task<Instituition> RegisterInstituition(Instituition instituition);
    Task<ICollection<InstituitionViewModel>> InstituitionsList();
    Task<Instituition> InstituitionEdit(Instituition instituition);
    Task<string> InstituitionsName(int? instituitionId);
    void DeleteInstituition(int id);
}
