using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.ViewModels.Configs;

namespace nScheduler.API.Controllers.Configs;

[Route("api/[controller]/[Action]")]
[Authorize(Roles = "Manager,Admin")]
public class ImageCfgController : Controller
{
    private readonly IImageCfgRepository repository;

    public ImageCfgController(IImageCfgRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<QueryResult<ImageCfgViewModel>> List(string name, CancellationToken cancellationToken = default)
    {
        var result = await repository.List(name.IsEmpty() ? x => true : x => x.Name.Contains(name), cancellationToken);

        return new QueryResult<ImageCfgViewModel>()
        {
            Total = 0,
            Items = result.Select(x =>
            {
                return new ImageCfgViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    ImageName = x.ImageName,
                    Configs = x.GetConfigs().Select(x => new ImageCfgParamViewModel() { ParamName = x.Key, ParamType = x.Value }).ToList(),
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpGet]
    public async Task<QueryResult<ImageCfgViewModel>> Page(string name, int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await repository.Page(name.IsEmpty() ? x => true : x => x.Name.Contains(name), page, size, cancellationToken);

        return new QueryResult<ImageCfgViewModel>()
        {
            Total = result.Item1,
            Items = result.Item2.Select(x =>
            {
                return new ImageCfgViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    ImageName = x.ImageName,
                    Configs = x.GetConfigs().Select(x => new ImageCfgParamViewModel() { ParamName = x.Key, ParamType = x.Value }).ToList(),
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpPost]
    public async Task<BaseResult> Edit([FromQuery] string id, [FromBody] ImageCfgEditViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Edit(new ImageCfg(id, model.Name, model.ImageName, model.ConfigParams, HttpContext.User!.Identity!.Name!),
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