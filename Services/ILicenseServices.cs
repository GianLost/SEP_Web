using SEP_Web.Models;

namespace SEP_Web.Services;
public interface ILicenseServices
{
    Task<ICollection<Licenses>> LicenseList();
}