using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.ViewModels.Configs;

namespace nScheduler.API.Controllers.Configs;

[Route("api/[controller]/[Action]")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserRepository repository;

    public UserController(IUserRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<QueryResult<UserViewModel>> Page(string name, int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await repository.Page(name.IsEmpty() ? x => true : x => x.UserName.Contains(name), page, size, cancellationToken);

        return new QueryResult<UserViewModel>()
        {
            Total = result.Item1,
            Items = result.Item2.Select(x =>
            {
                return new UserViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Password = x.Password,
                    Role = x.Role,
                    Email = x.Email,
                    Phone = x.Phone
                };
            })
        };
    }

    [HttpPost]
    public async Task<BaseResult> Edit([FromQuery] string id, [FromBody] UserEditViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Edit(new UserModel(id, model.UserName, model.IsPwdChanged ? model.Password : model.Password.Decode(), model.Role, model.Email, model.Phone, HttpContext.User!.Identity!.Name!),
                cancellationToken);

            return new BaseResult
            {
                Code = result ? 0 : -1,
                Msg = result ? "" : "删除数据失败"
            };
        }
        catch (Exception ex)
        {
            return new BaseResult
            {
                Code = -1,
                Msg = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<BaseResult> Delete([FromForm] string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Delete(id, cancellationToken);

            return new BaseResult
            {
                Code = result ? 0 : -1,
                Msg = result ? "" : "删除数据失败"
            };
        }
        catch (Exception ex)
        {
            return new BaseResult
            {
                Code = -1,
                Msg = ex.Message
            };
        }
    }
}