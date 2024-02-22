using SEP_Web.Models;

namespace SEP_Web.Services;
public interface ILicenseServices
{
    Licenses SearchForId(int id);
    Task<ICollection<Licenses>> LicenseList();
    Task<Licenses> RegisterLicense(Licenses license);
}