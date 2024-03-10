using nScheduler.Common.Extensions;
using nScheduler.Common.Models;

namespace nScheduler.Domain.Models.Configs;

public class UserModel : BaseAggregateRoot<string>
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public UserRole Role { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    private UserModel()
    { }

    public UserModel(string id, string userName, string password, UserRole role, string emial, string phone, string operName)
    {
        Id = id;
        UserName = userName;
        Role = role;
        Password = password.Encode();
        Email = emial;
        Phone = phone;
        OperName = operName;
        OperTime = DateTime.Now;
    }

    public bool CheckPwd(string pwd)
    {
        return Password == pwd.Encode();
    }
}