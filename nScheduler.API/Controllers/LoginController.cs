using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.ViewModels.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nScheduler.API.Controllers;

[Route("api/[controller]")]
public class LoginController : Controller
{
    private readonly IConfiguration configuration;
    private readonly IUserRepository repository;

    public LoginController(IConfiguration configuration, IUserRepository repository)
    {
        this.configuration = configuration;
        this.repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginViewModel login, CancellationToken cancellationToken)
    {
        try
        {
            var user = await repository.Find(login.UserName, cancellationToken);
            if (user == null) throw new ArgumentNullException("找不到相关用户");

            if (!user.CheckPwd(login.Password))
                throw new ArgumentNullException("用户名或密码错误");

            return Ok(new BaseResult
            {
                Code = 0,
                Msg = CreateJwtToken(user)
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseResult
            {
                Code = -1,
                Msg = ex.Message
            });
        }
    }

    private string CreateJwtToken(UserModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var config = configuration["JwtSecurityKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(Convert.ToInt32(configuration["JwtExpiryInDays"]));

        var token = new JwtSecurityToken(
            configuration["JwtIssuer"],
            configuration["JwtAudience"],
            claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}