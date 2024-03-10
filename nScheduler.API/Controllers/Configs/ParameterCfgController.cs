using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.ViewModels.Configs;

namespace nScheduler.API.Controllers.Configs;

[Route("api/[controller]/[Action]")]
[Authorize]
public class ParameterCfgController : Controller
{
    private readonly IParameterCfgRepository repository;

    public ParameterCfgController(IParameterCfgRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<QueryResult<ParameterCfgListViewModel>> List(string name, CancellationToken cancellationToken = default)
    {
        var result = await repository.List(name.IsEmpty() ? x => true : x => x.Name.Contains(name), cancellationToken);

        return new QueryResult<ParameterCfgListViewModel>()
        {
            Total = 0,
            Items = result.Select(x =>
            {
                return new ParameterCfgListViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    Content = x.Content,
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpGet]
    public async Task<QueryResult<ParameterCfgListViewModel>> Page(string name, int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await repository.Page(name.IsEmpty() ? x => true : x => x.Name.Contains(name), page, size, cancellationToken);

        return new QueryResult<ParameterCfgListViewModel>()
        {
            Total = result.Item1,
            Items = result.Item2.Select(x =>
            {
                return new ParameterCfgListViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    Content = x.Content,
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpPost]
    public async Task<BaseResult> Edit([FromQuery] string id, [FromBody] ParameterCfgEditViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Edit(new ParameterCfg(id, model.Name, model.Content, HttpContext.User!.Identity!.Name!),
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
            var result = await repository.Delete(id.ToGuid(), cancellationToken);

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