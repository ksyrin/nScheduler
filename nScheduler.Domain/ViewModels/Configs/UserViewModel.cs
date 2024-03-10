using nScheduler.Common.Models;
using System.ComponentModel;

namespace nScheduler.Domain.ViewModels.Configs;

public class UserViewModel
{
    [DisplayName("编码")]
    public string Id { get; set; }

    [DisplayName("用户名")]
    public string UserName { get; set; }

    public string Password { get; set; }

    public UserRole Role { get; set; }

    [DisplayName("角色")]
    public string RoleStr => Role switch
    {
        UserRole.Admin => "超级用户",
        UserRole.Manager => "管理员",
        UserRole.User => "普通用户",
        _ => "未知"
    };

    [DisplayName("邮箱")]
    public string Email { get; set; }

    [DisplayName("电话")]
    public string Phone { get; set; }

    public bool IsPwdChanged { get; set; } = false;
}