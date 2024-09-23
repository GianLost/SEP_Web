using SEP_Web.Keys;

namespace SEP_Web.ViewModels;

public class UsersViewModel
{
    public int Id { get; set; }
    public UserStatsEnum UserStats { get; set; }
    public int? Masp { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}