using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Common.Models;
using nScheduler.Domain.ViewModels.Jobs;
using nScheduler.Imp.Events.JobLog;

namespace nScheduler.API.Controllers.Jobs;

/// <summary>
/// 作业日志
/// </summary>
[Route("api/[controller]/[Action]")]
[ApiController]
[Authorize]
public class JobLogController : Controller
{
    private readonly IMediator mediator;

    public JobLogController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<JobLogViewModel?> Get([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobLogSignle { Id = id }, cancellationToken);
    }

    [HttpPost]
    public async Task<QueryResult<JobLogViewModel>> Page([FromBody] JobLogSearchViewModel viewModel, int page, int size, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new JobLogPage
        {
            Model = viewModel,
            Page = page,
            Size = size
        }, cancellationToken);

        return new QueryResult<JobLogViewModel>()
        {
            Total = response.Item1,
            Items = response.Item2
        };
    }

    [HttpPost]
    public async Task<BaseResult> Stop([FromForm] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobLogStop
        {
            Id = id
        }, cancellationToken);
    }
}