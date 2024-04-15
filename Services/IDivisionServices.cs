using SEP_Web.Models.StructuresModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Services;

public interface IDivisionServices
{
    Division SearchForId(int id);
    Task<Division> RegisterDivision(Division division);
    Task<ICollection<DivisionViewModel>> DivisionsList();
    Task<Division> DivisionEdit(Division division);
    Task<string> DivisionsName(int? divisionId);
    Task<ICollection<Division>> GetDivisionsAsync(int instituitionId);
    void DeleteDivision(int id);
}
