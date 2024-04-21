using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;

namespace nScheduler.Web.Shared;

public class BootstrapAppContext
{
    public BootstrapAppContext(AuthenticationState auth)
    {
        UserName = auth.User.Identity?.Name ?? string.Empty;
        UserId = auth.User.Claims!.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
        Email = auth.User.Claims!.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
        Phone = auth.User.Claims!.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)!.Value;
        var roleStr = auth.User.Claims!.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value;
        Role = Enum.Parse<UserRole>(roleStr);
    }


    public string UserId { get; init; }

    public string UserName { get; init; }

    public string Email { get; init; }

    public string Phone { get; init; }

    public UserRole Role { get; init; }

    public string UserRole => Role.GetEnumDescription();
}

