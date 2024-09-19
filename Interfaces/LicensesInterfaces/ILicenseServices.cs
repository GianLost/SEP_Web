using SEP_Web.Models.LicensesModels;

namespace SEP_Web.Interfaces.LicensesInterfaces;

public interface ILicenseServices
{
    Licenses SearchForId(int id);
    Task<ICollection<Licenses>> LicenseList();
    Task<Licenses> RegisterLicense(Licenses license);
    Task<Licenses> LicensesEdit(Licenses license);
    Task<int> GetMaxLicenseDuration(int licenseId);
    void DeleteLicenses(int id);
}