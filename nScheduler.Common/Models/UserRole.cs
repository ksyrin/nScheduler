using System.ComponentModel;

namespace nScheduler.Common.Models;

public enum UserRole
{
    [Description("普通用户")]
    User,

    [Description("管理员")]
    Manager,

    [Description("超级用户")]
    Admin
}