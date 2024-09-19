using SEP_Web.Models.LicensesModels;

namespace SEP_Web.Interfaces.LicensesInterfaces;

public interface IServantLicenseServices
{
    ServantLicense SearchForId(int id);
    Task<ICollection<ServantLicense>> ServantLicenseList();
    Task<ServantLicense> RegisterServantLicense(ServantLicense servantLicense);
    Task<ServantLicense> ServantLicensesEdit(ServantLicense servantLicense);
    void DeleteServantLicenses(int id);
}