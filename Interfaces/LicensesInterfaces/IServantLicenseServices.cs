using SEP_Web.Models.LicensesModels;
using SEP_Web.ViewModels;

namespace SEP_Web.Interfaces.LicensesInterfaces;

public interface IServantLicenseServices
{
    ServantLicense SearchForId(int id);
    Task<ICollection<ServantLicense>> ServantLicenseList();
    Task<ServantLicenseViewModel> GetByIdAsync(int id);
    IQueryable<ServantLicense> ServantLicensesAsQueryable();
    Task<ServantLicense> RegisterServantLicense(ServantLicense servantLicense);
    Task<ServantLicense> ServantLicensesEdit(ServantLicense servantLicense);
    void DeleteServantLicenses(int id);
}