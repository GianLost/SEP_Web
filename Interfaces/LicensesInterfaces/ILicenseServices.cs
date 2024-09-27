using SEP_Web.Models.LicensesModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.LicensesInterfaces;

public interface ILicenseServices
{
    Licenses SearchForId(int id);
    Task<ICollection<Licenses>> LicenseList();
    Task<LicenseViewModel> GetByIdAsync(int id);
    IQueryable<Licenses> LicensesAsQueryable();
    Task<Licenses> RegisterLicense(Licenses license);
    Task<Licenses> LicensesEdit(Licenses license);
    Task<int> GetMaxLicenseDuration(int licenseId);
    void DeleteLicenses(int id);
}