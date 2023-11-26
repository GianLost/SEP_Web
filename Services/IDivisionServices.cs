using SEP_Web.Models;

namespace SEP_Web.Services;
public interface IDivisionServices
{
    Division SearchForId(int id);
    Task<Division> RegisterDivision(Division division);
    Task<ICollection<Division>> DivisionsList();
    string DivisionsName(int? divisionId);
    Task<Division> DivisionEdit(Division division);
    Task<string> InstituitionName(Division divisionId);
    void DeleteDivision(int id);
}
