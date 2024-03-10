using nScheduler.Common.Models;

namespace nScheduler.Domain.ViewModels.Configs;

public class UserEditViewModel
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public UserRole Role { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public bool IsPwdChanged { get; set; } = false;
}