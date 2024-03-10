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
public class MessageCfgController : Controller
{
    private readonly IMessageCfgRepository repository;

    public MessageCfgController(IMessageCfgRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<QueryResult<MessageCfgViewModel>> List(string name, CancellationToken cancellationToken = default)
    {
        var result = await repository.List(name.IsEmpty() ? x => true : x => x.Name.Contains(name), cancellationToken);

        return new QueryResult<MessageCfgViewModel>()
        {
            Total = 0,
            Items = result.Select(x =>
            {
                return new MessageCfgViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    Url = x.Url,
                    MsgType = (int)x.Type,
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpGet]
    public async Task<QueryResult<MessageCfgViewModel>> Page(string name, int page, int size, CancellationToken cancellationToken = default)
    {
        var result = await repository.Page(name.IsEmpty() ? x => true : x => x.Name.Contains(name), page, size, cancellationToken);

        return new QueryResult<MessageCfgViewModel>()
        {
            Total = result.Item1,
            Items = result.Item2.Select(x =>
            {
                return new MessageCfgViewModel()
                {
                    Id = x.Id.ToStringN(),
                    Name = x.Name,
                    Url = x.Url,
                    MsgType = (int)x.Type,
                    OperName = x.OperName,
                    OperDate = x.OperTime.ToString("yyyy/MM/dd HH:mm:ss")
                };
            })
        };
    }

    [HttpPost]
    public async Task<BaseResult> Edit([FromQuery] string id, [FromBody] MessageCfgEditModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Edit(new MessageCfg(id, model.Name, model.Url, model.Type, HttpContext.User!.Identity!.Name!),
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