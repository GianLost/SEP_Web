using SEP_Web.Models.LicensesModels;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.ViewModels;
public class ServantLicenseViewModel
{
    public int Id { get; set; }
    public int? Masp { get; set; }
    public int CivilServantId { get; set; }
    public CivilServant CivilServant { get; set; }
    public int LicensesId { get; set; }
    public Licenses License { get; set; }
    public string Name { get; set; }
    public string LicenseName { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}